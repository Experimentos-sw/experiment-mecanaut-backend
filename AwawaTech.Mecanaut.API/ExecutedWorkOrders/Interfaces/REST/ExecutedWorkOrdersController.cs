using System.Linq;
using System.Threading.Tasks;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Domain.Services;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Interfaces.REST.Resources;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using AwawaTech.Mecanaut.API.Shared.Domain.Services;


namespace AwawaTech.Mecanaut.API.ExecutedWorkOrders.Interfaces.REST;

[ApiController]
[Route("api/v1/executed-work-orders")]
public class ExecutedWorkOrdersController : ControllerBase
{
    private readonly IExecutedWorkOrderCommandService _commandService;
    private readonly IExecutedWorkOrderQueryService _queryService;

    public ExecutedWorkOrdersController(
        IExecutedWorkOrderCommandService commandService,
        IExecutedWorkOrderQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] SaveExecutedWorkOrderResource resource)
    {
        var command = SaveExecutedWorkOrderCommandFromResourceAssembler.ToCommand(resource);
        var executedWorkOrder = await _commandService.HandleAsync(command, resource.Files, resource.WorkOrderId);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        var executedWorkOrder = await _queryService.FindByIdAsync(id);
        if (executedWorkOrder == null)
            return NotFound();

        var usedProducts = await _queryService.FindUsedProductsByExecutedWorkOrderIdAsync(id);
        var executionImages = await _queryService.FindImagesByExecutedWorkOrderIdAsync(id);
        var resource = ExecutedWorkOrderToResourceAssembler.ToResource(executedWorkOrder, usedProducts, executionImages);
        return Ok(resource);
    }

    [HttpGet("production-line/{lineId}")]
    public async Task<IActionResult> GetByProductionLineIdAsync(long lineId)
    {
        var executedWorkOrders = await _queryService.FindByProductionLineIdAsync(lineId);
        var allUsedProducts = await _queryService.FindUsedProductsByExecutedWorkOrderIdsAsync(
            executedWorkOrders.Select(x => x.Id));
        
        var allExecutionImages = await _queryService.FindImagesByExecutedWorkOrderIdsAsync(
            executedWorkOrders.Select(x => x.Id));
        
        var resources = executedWorkOrders.Select(order => 
            ExecutedWorkOrderToResourceAssembler.ToResource(
                order, 
                allUsedProducts.Where(p => p.ExecutedWorkOrderId == order.Id),
                allExecutionImages));
                
        return Ok(resources);
    }
} 

