using System.Threading.Tasks;
using AwawaTech.Mecanaut.API.Shared.Domain.Repositories;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Repositories;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Services;

namespace AwawaTech.Mecanaut.API.WorkOrders.Application.Internal.CommandServices;

public class WorkOrderCommandService : IWorkOrderCommandService
{
    private readonly IWorkOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public WorkOrderCommandService(IWorkOrderRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
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
} 