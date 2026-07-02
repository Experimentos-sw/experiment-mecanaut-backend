using System.Linq;
using System.Threading.Tasks;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Interfaces.REST;

[ApiController]
[Route("api/v1/dynamic-maintenance-plans/kpi-projections")]
public class PlanKpiProjectionsController : ControllerBase
{
    private readonly IMaintenanceKpiQueryService _maintenanceKpiQueryService;

    public PlanKpiProjectionsController(IMaintenanceKpiQueryService maintenanceKpiQueryService)
    {
        _maintenanceKpiQueryService = maintenanceKpiQueryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjection([FromQuery] long[] machineIds, [FromQuery] int metricId = 1, [FromQuery] double amount = 0, [FromQuery] int taskCount = 0)
    {
        if (machineIds == null || !machineIds.Any())
            return BadRequest("machineIds are required.");

        var projection = await _maintenanceKpiQueryService.ProjectKpisAsync(machineIds, metricId, amount, taskCount);

        return Ok(new
        {
            mtbf = projection.MtbfHours,
            mttr = projection.MttrHours,
            unit = "hours"
        });
    }
}
