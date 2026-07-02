using AwawaTech.Mecanaut.API.FormContact.Application.Internal.CommandServices;
using AwawaTech.Mecanaut.API.FormContact.Application.Internal.QueryServices;
using AwawaTech.Mecanaut.API.FormContact.Domain.Services;
using AwawaTech.Mecanaut.API.FormContact.Interfaces.REST.Resources;
using AwawaTech.Mecanaut.API.FormContact.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AwawaTech.Mecanaut.API.FormContact.Interfaces.REST;

[ApiController]
[Route("api/v1/support-requests")]
public class SupportRequestsController : ControllerBase
{
    private readonly ISupportRequestCommandService _commandService;
    private readonly ISupportRequestQueryService _queryService;

    public SupportRequestsController(
        ISupportRequestCommandService commandService,
        ISupportRequestQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<SupportRequestResource>> CreateAsync([FromBody] CreateSupportRequestResource resource)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = CreateSupportRequestCommandFromResourceAssembler.ToCommand(resource);
        var supportRequest = await _commandService.HandleAsync(command);
        var result = SupportRequestToResourceAssembler.ToResource(supportRequest);

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SupportRequestResource>> GetByIdAsync(long id)
    {
        var supportRequest = await _queryService.FindByIdAsync(id);
        if (supportRequest == null)
        {
            return NotFound();
        }

        return Ok(SupportRequestToResourceAssembler.ToResource(supportRequest));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupportRequestResource>>> GetAllAsync()
    {
        var supportRequests = await _queryService.ListAsync();
        var resources = supportRequests.Select(SupportRequestToResourceAssembler.ToResource);
        return Ok(resources);
    }
}
