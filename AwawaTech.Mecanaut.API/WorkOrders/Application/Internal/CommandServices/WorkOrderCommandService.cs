using System.Threading.Tasks;
using System.Linq;
using AwawaTech.Mecanaut.API.Shared.Domain.Repositories;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Repositories;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Services;
using AwawaTech.Mecanaut.API.InventoryManagement.Domain.Services;
using AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.Queries;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects;

namespace AwawaTech.Mecanaut.API.WorkOrders.Application.Internal.CommandServices;

public class WorkOrderCommandService : IWorkOrderCommandService
{
    private readonly IWorkOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IInventoryPartQueryService _inventoryPartQueryService;

    public WorkOrderCommandService(IWorkOrderRepository repository, IUnitOfWork unitOfWork, IInventoryPartQueryService inventoryPartQueryService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _inventoryPartQueryService = inventoryPartQueryService;
    }

    public async Task<WorkOrder> Handle(CreateWorkOrderCommand command)
    {
        var workOrder = WorkOrder.Create(
            command.Code,
            command.TenantId,
            command.Date,
            command.ProductionLineId,
            command.Type);

        workOrder.AssignMachines(command.MachineIds);
        workOrder.AddTasks(command.Tasks);
        
        if (command.TechnicianIds?.Count > 0)
        {
            workOrder.AddTechnicians(command.TechnicianIds);
        }

        if (command.RequiredParts?.Count > 0)
        {
            var parts = command.RequiredParts.Select(p => new WorkOrderRequiredPart(p.InventoryPartId, p.Quantity)).ToList();
            workOrder.AddRequiredParts(parts);
        }

        await _repository.AddAsync(workOrder);
        await _unitOfWork.CompleteAsync();
        return workOrder;
    }

    public async Task<WorkOrder> Handle(CompleteWorkOrderCommand command)
    {


        var workOrder = await _repository.FindByIdAsync(command.WorkOrderId, command.TenantId);
        if (workOrder == null) return null;

        workOrder.Complete();
        await _unitOfWork.CompleteAsync();
        return workOrder;
    }

    public async Task<WorkOrder> Handle(AssignTechniciansCommand command)
    {
        var workOrder = await _repository.FindByIdAsync(command.WorkOrderId, command.TenantId);
        if (workOrder == null) return null;

        workOrder.AddTechnicians(command.TechnicianIds);
        _repository.Update(workOrder);  // Aseguramos que EF detecte el cambio en la colección
        await _unitOfWork.CompleteAsync();
        return workOrder;
    }

    public async Task<StartWorkOrderResult> Handle(StartWorkOrderCommand command)
    {
        var result = new StartWorkOrderResult();

        var workOrder = await _repository.FindByIdAsync(command.WorkOrderId, command.TenantId);
        if (workOrder == null)
        {
            result.Success = false;
            return result;
        }

        if (workOrder.Status != WorkOrderStatus.Pending)
        {
            result.Success = false;
            return result;
        }

        // Validar stock de cada repuesto asignado en tiempo real
        foreach (var requiredPart in workOrder.RequiredParts)
        {
            var partQuery = new GetInventoryPartByIdQuery(requiredPart.InventoryPartId);
            var inventoryPart = await _inventoryPartQueryService.Handle(partQuery);

            if (inventoryPart == null || inventoryPart.CurrentStock < requiredPart.Quantity)
            {
                result.MissingParts.Add(new MissingPartInfo
                {
                    InventoryPartId = requiredPart.InventoryPartId,
                    PartNumber = inventoryPart?.PartNumber ?? "N/A",
                    Name = inventoryPart?.Name ?? "Desconocido",
                    RequiredQuantity = requiredPart.Quantity,
                    AvailableQuantity = inventoryPart?.CurrentStock ?? 0
                });
            }
        }

        if (result.MissingParts.Any())
        {
            result.Success = false;
            return result;
        }

        // Si hay stock suficiente para todos los repuestos, iniciamos la orden
        workOrder.Start();
        _repository.Update(workOrder);
        await _unitOfWork.CompleteAsync();

        result.Success = true;
        result.WorkOrder = workOrder;
        return result;
    }
} 