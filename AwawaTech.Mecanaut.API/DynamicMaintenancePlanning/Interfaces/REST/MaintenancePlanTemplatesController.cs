using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Application.Internal.CommandServices;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Application.Internal.QueryServices;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Services;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Interfaces.REST.Resources;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Interfaces.REST;

[ApiController]
[Route("api/v1/maintenance-plan-templates")]
public class MaintenancePlanTemplatesController : ControllerBase
{
    private readonly IMaintenancePlanTemplateCommandService _commandService;
    private readonly IMaintenancePlanTemplateQueryService _queryService;
    private readonly CreateMaintenancePlanTemplateCommandFromResourceAssembler _fromResourceAssembler;
    private readonly MaintenancePlanTemplateToResourceAssembler _toResourceAssembler;

    public MaintenancePlanTemplatesController(
        IMaintenancePlanTemplateCommandService commandService,
        IMaintenancePlanTemplateQueryService queryService,
        CreateMaintenancePlanTemplateCommandFromResourceAssembler fromResourceAssembler,
        MaintenancePlanTemplateToResourceAssembler toResourceAssembler)
    {
        _commandService = commandService;
        _queryService = queryService;
        _fromResourceAssembler = fromResourceAssembler;
        _toResourceAssembler = toResourceAssembler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaintenancePlanTemplateResource>>> GetAllAsync()
    {
        var templates = await _queryService.GetAllAsync();
        var resources = templates.Select(_toResourceAssembler.ToResource);
        return Ok(resources);
    }

    [HttpPost]
    public async Task<ActionResult<MaintenancePlanTemplateResource>> CreateAsync([FromBody] CreateMaintenancePlanTemplateResource resource)
    {
        if (resource == null)
            return BadRequest("Invalid body.");

        var command = _fromResourceAssembler.ToCommand(resource);
        var template = await _commandService.CreateAsync(command);

        var response = _toResourceAssembler.ToResource(template);
        return Ok(response);
    }
}
