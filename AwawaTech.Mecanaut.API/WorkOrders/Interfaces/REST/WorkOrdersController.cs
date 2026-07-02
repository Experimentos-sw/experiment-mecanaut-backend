using System;
using System.Linq;
using System.Threading.Tasks;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Queries;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Services;
using AwawaTech.Mecanaut.API.WorkOrders.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwawaTech.Mecanaut.API.WorkOrders.Interfaces.REST;

[ApiController]
[Route("api/v1/work-orders")]
[Authorize] // Aseguramos que el usuario esté autenticado
public class WorkOrdersController : ControllerBase
{
    private readonly IWorkOrderCommandService _commandService;
    private readonly IWorkOrderQueryService _queryService;

    public WorkOrdersController(
        IWorkOrderCommandService commandService,
        IWorkOrderQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpPost]
    public async Task<ActionResult<WorkOrderResource>> CreateWorkOrder([FromBody] CreateWorkOrderResource resource)
    {
        var tenantIdStr = User.Claims.FirstOrDefault(c => c.Type == "tenant_id")?.Value;
        if (string.IsNullOrEmpty(tenantIdStr) || !long.TryParse(tenantIdStr, out var tenantIdValue))
        {
            return Unauthorized("No tenant ID found in token");
        }

        var command = new CreateWorkOrderCommand
        {
            Code = resource.Code,
            TenantId = new TenantId(tenantIdValue),
            Date = resource.Date,
            ProductionLineId = resource.ProductionLineId,
            Type = Enum.Parse<WorkOrderType>(resource.Type),
            MachineIds = resource.MachineIds,
            Tasks = resource.Tasks,
            TechnicianIds = resource.TechnicianIds
        };

        var workOrder = await _commandService.Handle(command);
        var response = new WorkOrderResource(
            workOrder.Id,
            workOrder.Code,
            workOrder.Status.ToString(),
            workOrder.Type.ToString(),
            workOrder.Date,
            workOrder.ProductionLineId,
            workOrder.MachineIds,
            workOrder.TechnicianIds,
            workOrder.Tasks
        );

        return CreatedAtAction(
            nameof(GetWorkOrderById),
            new { id = response.Id },
            response);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<WorkOrderResource>> GetWorkOrderById(long id)
    {
        var tenantIdStr = User.Claims.FirstOrDefault(c => c.Type == "tenant_id")?.Value;
        if (string.IsNullOrEmpty(tenantIdStr) || !long.TryParse(tenantIdStr, out var tenantIdValue))
        {
            return Unauthorized("No tenant ID found in token");
        }

        var query = new GetWorkOrderByIdQuery
        {
            Id = id,
            TenantId = new TenantId(tenantIdValue)
        };

        var workOrder = await _queryService.Handle(query);
        if (workOrder == null)
            return NotFound();

        var response = new WorkOrderResource(
            workOrder.Id,
            workOrder.Code,
            workOrder.Status.ToString(),
            workOrder.Type.ToString(),
            workOrder.Date,
            workOrder.ProductionLineId,
            workOrder.MachineIds,
            workOrder.TechnicianIds,
            workOrder.Tasks
        );

        return Ok(response);
    }

    [HttpGet("by-production-line/{productionLineId:long}")]
    public async Task<ActionResult<IEnumerable<WorkOrderResource>>> GetWorkOrdersByProductionLine(long productionLineId)
    {
        var tenantIdStr = User.Claims.FirstOrDefault(c => c.Type == "tenant_id")?.Value;
        if (string.IsNullOrEmpty(tenantIdStr) || !long.TryParse(tenantIdStr, out var tenantIdValue))
        {
            return Unauthorized("No tenant ID found in token");
        }

        var query = new GetWorkOrdersByProductionLineQuery
        {
            ProductionLineId = productionLineId,
            TenantId = new TenantId(tenantIdValue)
        };

        var workOrders = await _queryService.Handle(query);
        
        var response = workOrders.Select(workOrder => new WorkOrderResource(
            workOrder.Id,
            workOrder.Code,
            workOrder.Status.ToString(),
            workOrder.Type.ToString(),
            workOrder.Date,
            workOrder.ProductionLineId,
            workOrder.MachineIds,
            workOrder.TechnicianIds,
            workOrder.Tasks
        ));

        return Ok(response);
    }

    
    [HttpGet("by-production-line-to-execute/{productionLineId:long}")]
    public async Task<ActionResult<IEnumerable<WorkOrderResource>>> GetWorkOrdersByProductionLineToExecute(long productionLineId)
    {
        var tenantIdStr = User.Claims.FirstOrDefault(c => c.Type == "tenant_id")?.Value;
        if (string.IsNullOrEmpty(tenantIdStr) || !long.TryParse(tenantIdStr, out var tenantIdValue))
        {
            return Unauthorized("No tenant ID found in token");
        }

        var query = new GetWorkOrdersByProductionLineQuery
        {
            ProductionLineId = productionLineId,
            TenantId = new TenantId(tenantIdValue)
        };

        var workOrders = await _queryService.GetTo(query);
        
        var response = workOrders.Select(workOrder => new WorkOrderResource(
            workOrder.Id,
            workOrder.Code,
            workOrder.Status.ToString(),
            workOrder.Type.ToString(),
            workOrder.Date,
            workOrder.ProductionLineId,
            workOrder.MachineIds,
            workOrder.TechnicianIds,
            workOrder.Tasks
        ));

        return Ok(response);
    }

    
    
    [HttpPut("{id:long}/complete")]
    public async Task<ActionResult<WorkOrderResource>> CompleteWorkOrder(long id, [FromBody] CompleteWorkOrderResource resource)
    {
        var tenantIdStr = User.Claims.FirstOrDefault(c => c.Type == "tenant_id")?.Value;
        if (string.IsNullOrEmpty(tenantIdStr) || !long.TryParse(tenantIdStr, out var tenantIdValue))
        {
            return Unauthorized("No tenant ID found in token");
        }

        var command = new CompleteWorkOrderCommand
        {
            WorkOrderId = id,
            TenantId = new TenantId(tenantIdValue),
            IsAreaCleaned = resource.IsAreaCleaned,
            AreToolsReturned = resource.AreToolsReturned,
            IsOperationsVerified = resource.IsOperationsVerified
        };

        var workOrder = await _commandService.Handle(command);
        if (workOrder == null)
            return NotFound();

        var response = new WorkOrderResource(
            workOrder.Id,
            workOrder.Code,
            workOrder.Status.ToString(),
            workOrder.Type.ToString(),
            workOrder.Date,
            workOrder.ProductionLineId,
            workOrder.MachineIds,
            workOrder.TechnicianIds,
            workOrder.Tasks
        );

        return Ok(response);
    }

    [HttpPut("{id:long}/technicians")]
    public async Task<ActionResult<WorkOrderResource>> AssignTechnicians(long id, [FromBody] List<long?> technicianIds)
    {
        var tenantIdStr = User.Claims.FirstOrDefault(c => c.Type == "tenant_id")?.Value;
        if (string.IsNullOrEmpty(tenantIdStr) || !long.TryParse(tenantIdStr, out var tenantIdValue))
        {
            return Unauthorized("No tenant ID found in token");
        }

        var command = new AssignTechniciansCommand
        {
            WorkOrderId = id,
            TenantId = new TenantId(tenantIdValue),
            TechnicianIds = technicianIds
        };

        var workOrder = await _commandService.Handle(command);
        if (workOrder == null)
            return NotFound();

        var response = new WorkOrderResource(
            workOrder.Id,
            workOrder.Code,
            workOrder.Status.ToString(),
            workOrder.Type.ToString(),
            workOrder.Date,
            workOrder.ProductionLineId,
            workOrder.MachineIds,
            workOrder.TechnicianIds,
            workOrder.Tasks
        );

        return Ok(response);
    }
    
    
} 