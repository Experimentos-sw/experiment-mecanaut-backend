using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AwawaTech.Mecanaut.API.Shared.Domain.Repositories;
using AwawaTech.Mecanaut.API.TelemetryManagement.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using AwawaTech.Mecanaut.API.TelemetryManagement.Interfaces.REST.Resources;

namespace AwawaTech.Mecanaut.API.TelemetryManagement.Interfaces.REST
{
    [ApiController]
    [Route("api/v1/experiment-telemetry")]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class ExperimentTelemetryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context; // O usar patrón Repositorio si lo prefieres

        public ExperimentTelemetryController(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RecordMetric([FromBody] TelemetryResource resource)
        {
            var log = new ExperimentLog(
                resource.ExperimentName,
                resource.Variant,
                resource.ActionType,
                resource.DurationMilliseconds,
                resource.IsSuccess,
                resource.AdditionalData
            );

            await _context.ExperimentLogs.AddAsync(log);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Métrica del experimento registrada exitosamente." });
        }
    }
}
