using Microsoft.AspNetCore.Mvc;
using AwawaTech.Mecanaut.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;

namespace AwawaTech.Mecanaut.API.Shared.Interfaces.REST.Controllers;

[ApiController]
[Route("health")]
[Route("api/v1/health")]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult HealthCheck()
    {
        return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
    }
}
