using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using AwawaTech.Mecanaut.Tests.Integration;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace AwawaTech.Mecanaut.Tests.Integration;

public class AuthIntegrationTests : IClassFixture<MecanautWebApplicationFactory>
{
    private readonly HttpClient _client;
    private const string Password = "SecurePassword123!";
    private const string TokenSecret = "Place here your secret for token generation";

    public AuthIntegrationTests(MecanautWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task SignIn_ReturnsToken_WithExpectedClaims()
    {
        var signIn = await EnsureAuthenticatedSignIn();
        signIn.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = await signIn.Content.ReadFromJsonAsync<JsonElement>();

        var token = json.GetProperty("token").GetString();
        token.Should().NotBeNullOrWhiteSpace();

        var payload = DecodeJwtPayload(token!);
        payload.Should().ContainKey("tenant_id");
        payload.Should().Contain(kvp => kvp.Key.Contains("name", StringComparison.OrdinalIgnoreCase));
        payload.Should().Contain(kvp => kvp.Key.Contains("sid", StringComparison.OrdinalIgnoreCase));
        payload.Should().Contain(kvp => kvp.Key.Contains("role", StringComparison.OrdinalIgnoreCase) && kvp.Value.Contains("RoleAdmin"));
    }

    [Fact]
    public async Task FreshToken_Allows_UsersById_And_Plants()
    {
        var signIn = await EnsureAuthenticatedSignIn();
        var json = await signIn.Content.ReadFromJsonAsync<JsonElement>();
        var token = json.GetProperty("token").GetString()!;
        var userId = json.GetProperty("id").GetInt32();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var userResp = await _client.GetAsync($"/api/v1/users/{userId}");
        userResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var plantsResp = await _client.GetAsync("/api/v1/plants");
        plantsResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RoleAdmin_IsRecognized_OnRolesEndpoint()
    {
        var signIn = await EnsureAuthenticatedSignIn();
        var json = await signIn.Content.ReadFromJsonAsync<JsonElement>();
        var token = json.GetProperty("token").GetString()!;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var rolesResp = await _client.GetAsync("/api/v1/roles");

        rolesResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task TokenWithMissingUser_Returns401()
    {
        var signIn = await EnsureAuthenticatedSignIn();
        var json = await signIn.Content.ReadFromJsonAsync<JsonElement>();
        var token = json.GetProperty("token").GetString()!;
        var payload = DecodeJwtPayload(token);

        var forged = BuildToken(
            sid: "999999",
            name: payload.GetValueOrDefault("name", "admin@mecanaut.com"),
            tenantId: payload.GetValueOrDefault("tenant_id", "1"),
            roles: ExtractRoles(payload));

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", forged);
        var resp = await _client.GetAsync("/api/v1/plants");

        resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task TokenWithWrongTenant_Returns401()
    {
        var signIn = await EnsureAuthenticatedSignIn();
        var json = await signIn.Content.ReadFromJsonAsync<JsonElement>();
        var token = json.GetProperty("token").GetString()!;
        var payload = DecodeJwtPayload(token);

        var forged = BuildToken(
            sid: payload.GetValueOrDefault("sid", "1"),
            name: payload.GetValueOrDefault("name", "admin@mecanaut.com"),
            tenantId: "999999",
            roles: ExtractRoles(payload));

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", forged);
        var resp = await _client.GetAsync("/api/v1/plants");

        resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private async Task<HttpResponseMessage> EnsureAuthenticatedSignIn()
    {
        var signIn = await SignIn("admin@mecanaut.com");
        if (signIn.IsSuccessStatusCode) return signIn;

        var signUpPayload = new
        {
            ruc = "12345678901",
            legalName = "Test Company",
            tenantEmail = "test@company.com",
            subscriptionPlanId = 1,
            username = "admin@mecanaut.com",
            password = Password,
            email = "admin@mecanaut.com",
            firstName = "Admin",
            lastName = "User"
        };

        await _client.PostAsJsonAsync("/api/v1/authentication/sign-up", signUpPayload);
        return await SignIn("admin@mecanaut.com");
    }

    private async Task<HttpResponseMessage> SignIn(string username)
    {
        return await _client.PostAsJsonAsync("/api/v1/authentication/sign-in", new
        {
            username,
            password = Password
        });
    }

    private static Dictionary<string, string> DecodeJwtPayload(string token)
    {
        var parts = token.Split('.');
        var payloadBytes = Base64UrlEncoder.DecodeBytes(parts[1]);
        using var doc = JsonDocument.Parse(payloadBytes);
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var prop in doc.RootElement.EnumerateObject())
        {
            result[prop.Name] = prop.Value.ValueKind switch
            {
                JsonValueKind.Array => string.Join(",", prop.Value.EnumerateArray().Select(v => v.ToString())),
                _ => prop.Value.ToString()
            };
        }
        return result;
    }

    private static string[] ExtractRoles(Dictionary<string, string> payload)
    {
        var roleEntry = payload.FirstOrDefault(kvp => kvp.Key.Contains("role", StringComparison.OrdinalIgnoreCase));
        if (string.IsNullOrWhiteSpace(roleEntry.Value)) return [];
        return roleEntry.Value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    private static string BuildToken(string sid, string name, string tenantId, IEnumerable<string> roles)
    {
        var key = Encoding.ASCII.GetBytes(TokenSecret);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid, sid),
                new Claim(ClaimTypes.Name, name),
                new Claim("tenant_id", tenantId)
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        foreach (var role in roles)
            descriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }
}
