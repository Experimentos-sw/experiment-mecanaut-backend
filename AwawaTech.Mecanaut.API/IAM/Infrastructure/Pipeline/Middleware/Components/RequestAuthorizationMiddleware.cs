using AwawaTech.Mecanaut.API.IAM.Application.Internal.OutboundServices;
using AwawaTech.Mecanaut.API.IAM.Domain.Model.Queries;
using AwawaTech.Mecanaut.API.IAM.Domain.Services;
using AwawaTech.Mecanaut.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Multitenancy;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AwawaTech.Mecanaut.API.IAM.Infrastructure.Pipeline.Middleware.Components;

/**
 * RequestAuthorizationMiddleware is a custom middleware.
 * This middleware is used to authorize requests.
 * It validates a token is included in the request header and that the token is valid.
 * If the token is valid then it sets the user in HttpContext.Items["User"].
 */
public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    /**
     * InvokeAsync is called by the ASP.NET Core runtime.
     * It is used to authorize requests.
     * It validates a token is included in the request header and that the token is valid.
     * If the token is valid then it sets the user in HttpContext.Items["User"].
     */
    public async Task InvokeAsync(
        HttpContext context,
        IUserQueryService userQueryService,
        ITokenService tokenService,
        ILogger<RequestAuthorizationMiddleware> logger)
    {
        var endpoint = context.GetEndpoint();
        var allowAnonymous = endpoint?.Metadata.Any(m => m is AllowAnonymousAttribute) == true;
        if (allowAnonymous)
        {
            await next(context);
            return;
        }

        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token == null)
            throw new UnauthorizedAccessException("El encabezado Authorization es obligatorio.");

        var claims = await tokenService.ValidateToken(token);
        if (claims == null)
            throw new UnauthorizedAccessException("Token inválido o expirado.");

        var (userId, tenantId) = claims.Value;
        logger.LogInformation("Auth claims detected: userId={UserId}, tenantId={TenantId}", userId, tenantId);

        TenantContext.SetTenantId(tenantId);

        try
        {
            var getUserByIdQuery = new GetUserByIdQuery(userId);
            var user = await userQueryService.Handle(getUserByIdQuery);

            if (user == null)
            {
                logger.LogWarning("Authorization rejected. userFound=false, userId={UserId}, tenantId={TenantId}", userId, tenantId);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \"Usuario no encontrado o token inválido para este entorno.\"}");
                return;
            }

            logger.LogInformation(
                "Authorization resolved user. userFound=true, userId={UserId}, username={Username}, tenantId={TenantId}, roles=[{Roles}]",
                user.Id,
                user.Username,
                tenantId,
                string.Join(",", user.Roles.Select(r => r.Name.ToString())));

            var principalClaims = new List<Claim>
            {
                new(ClaimTypes.Sid, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new("tenant_id", tenantId.ToString())
            };
            principalClaims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name.ToString())));

            var identity = new ClaimsIdentity(principalClaims, "Custom");
            context.User = new ClaimsPrincipal(identity);
            context.Items["User"] = user;

            await next(context);
        }
        finally
        {
            TenantContext.Clear();
        }
    }
}
