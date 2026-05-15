using AwawaTech.Mecanaut.API.IAM.Application.Internal.OutboundServices;
using AwawaTech.Mecanaut.API.IAM.Domain.Model.Queries;
using AwawaTech.Mecanaut.API.IAM.Domain.Services;
using AwawaTech.Mecanaut.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Multitenancy;
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
        ITokenService tokenService)
    {
        Console.WriteLine("Entering InvokeAsync");
        // skip authorization if endpoint is decorated with [AllowAnonymous] attribute
        var endpoint = context.GetEndpoint();
        var allowAnonymous = endpoint?.Metadata
            .Any(m => m is AllowAnonymousAttribute) == true;
        Console.WriteLine($"Allow Anonymous is {allowAnonymous}");
        if (allowAnonymous)
        {
            Console.WriteLine("Skipping authorization");
            // [AllowAnonymous] attribute is set, so skip authorization
            await next(context);
            return;
        }
        Console.WriteLine("Entering authorization");
        // get token from request header
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();


        // if token is null then throw exception
        if (token == null) throw new UnauthorizedAccessException("El encabezado Authorization es obligatorio.");

        // validate token
        var claims = await tokenService.ValidateToken(token);

        // if token is invalid then throw exception
        if (claims == null) throw new UnauthorizedAccessException("Token inválido o expirado.");

        var (userId, tenantId) = claims.Value;

        // Establecer contexto de tenant lo antes posible
        TenantContext.SetTenantId(tenantId);

        try
        {
            // get user by id
            var getUserByIdQuery = new GetUserByIdQuery(userId);

            // obtener el usuario desde la capa de aplicación
            var user = await userQueryService.Handle(getUserByIdQuery);

            //prueba para que el back funcione
            if (user == null)
            {
                Console.WriteLine($"Fallo de autorización: No se encontró el usuario {userId} para el tenant {tenantId}.");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \"Usuario no encontrado o token inválido para este entorno.\"}");
                return; // Muy importante el return para cortar el flujo aquí
            }
            // construir ClaimsPrincipal con roles y tenant
            var claimsPrincipal = new List<Claim>
            {
                new(ClaimTypes.Sid, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new("tenant_id", tenantId.ToString())
            };
            claimsPrincipal.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name.ToString())));
            var identity = new ClaimsIdentity(claimsPrincipal, "Custom");
            context.User = new ClaimsPrincipal(identity);

            // guardar user para capas superiores (atributo Authorize custom)
            context.Items["User"] = user;
            Console.WriteLine("Successful authorization. Updating Context...");
            Console.WriteLine("Continuing with Middleware Pipeline");

            // call next middleware
            await next(context);
        }
        finally
        {
            // Limpiar el contexto al final de la petición
            TenantContext.Clear();
        }
    }
}
