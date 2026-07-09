using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Queries;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Services;
using AwawaTech.Mecanaut.API.WorkOrders.Interfaces.REST.Resources;
using AwawaTech.Mecanaut.API.InventoryManagement.Domain.Services;
using AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.Queries;
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
    private readonly IInventoryPartQueryService _inventoryQueryService;

    public WorkOrdersController(
        IWorkOrderCommandService commandService,
        IWorkOrderQueryService queryService,
        IInventoryPartQueryService inventoryQueryService)
    {
        _commandService = commandService;
        _queryService = queryService;
        _inventoryQueryService = inventoryQueryService;
    }

    private WorkOrderResource MapToResource(WorkOrder workOrder)
    {
        return new WorkOrderResource(
            workOrder.Id,
            workOrder.Code,
            workOrder.Status.ToString(),
            workOrder.Type.ToString(),
            workOrder.Date,
            workOrder.ProductionLineId,
            workOrder.MachineIds,
            workOrder.TechnicianIds,
            workOrder.Tasks,
            workOrder.RequiredParts?.Select(p => new RequiredPartResource(p.InventoryPartId, p.Quantity)).ToList() ?? new()
        );
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
            TechnicianIds = resource.TechnicianIds,
            RequiredParts = resource.RequiredParts?.Select(p => new RequiredPartCommand(p.InventoryPartId, p.Quantity)).ToList() ?? new()
        };

        var workOrder = await _commandService.Handle(command);
        var response = MapToResource(workOrder);

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

        return Ok(MapToResource(workOrder));
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
        var response = workOrders.Select(MapToResource);

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
        var response = workOrders.Select(MapToResource);

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
            TenantId = new TenantId(tenantIdValue)
        };

        var workOrder = await _commandService.Handle(command);
        if (workOrder == null)
            return NotFound();

        return Ok(MapToResource(workOrder));
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

        return Ok(MapToResource(workOrder));
    }

    [HttpGet("{id:long}/stock-verification")]
    public async Task<ActionResult<IEnumerable<StockVerificationResultResource>>> VerifyStock(long id)
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

        var results = new List<StockVerificationResultResource>();

        foreach (var requiredPart in workOrder.RequiredParts ?? new List<WorkOrderRequiredPart>())
        {
            var partQuery = new GetInventoryPartByIdQuery(requiredPart.InventoryPartId);
            var inventoryPart = await _inventoryQueryService.Handle(partQuery);

            results.Add(new StockVerificationResultResource(
                requiredPart.InventoryPartId,
                inventoryPart?.PartNumber ?? "N/A",
                inventoryPart?.Name ?? "Desconocido",
                requiredPart.Quantity,
                inventoryPart?.CurrentStock ?? 0,
                inventoryPart != null && inventoryPart.CurrentStock >= requiredPart.Quantity
            ));
        }

        return Ok(results);
    }

    [HttpPut("{id:long}/start")]
    public async Task<ActionResult<WorkOrderResource>> StartWorkOrder(long id)
    {
        var tenantIdStr = User.Claims.FirstOrDefault(c => c.Type == "tenant_id")?.Value;
        if (string.IsNullOrEmpty(tenantIdStr) || !long.TryParse(tenantIdStr, out var tenantIdValue))
        {
            return Unauthorized("No tenant ID found in token");
        }

        var command = new StartWorkOrderCommand
        {
            WorkOrderId = id,
            TenantId = new TenantId(tenantIdValue)
        };

        var result = await _commandService.Handle(command);

        if (result.MissingParts.Any())
        {
            return BadRequest(new { message = "No hay suficiente stock para iniciar la orden de trabajo.", missingParts = result.MissingParts });
        }

        if (!result.Success || result.WorkOrder == null)
        {
            return BadRequest("No se pudo iniciar la orden de trabajo. Asegúrese de que esté en estado Pendiente.");
        }

        return Ok(MapToResource(result.WorkOrder));
    }
} 