using AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.InventoryManagement.Domain.Services;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Application.Internal.OutboundServices;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;

using AwawaTech.Mecanaut.API.WorkOrders.Domain.Services;

namespace AwawaTech.Mecanaut.API.ExecutedWorkOrders.Infrastructure.OutboundServices.Services;


public class WorkOrderExecAcl : IWorkOrderExecAcl
{
    private readonly IWorkOrderCommandService _workOrderExecCommandService;

    public WorkOrderExecAcl (IWorkOrderCommandService workOrderExecCommandService)
    {
        _workOrderExecCommandService = workOrderExecCommandService;
    }

    public async Task MarkAsCompletedAsync(long workOrderId, TenantId tenantId)
    {
        var command = new CompleteWorkOrderCommand
        {
            WorkOrderId = workOrderId,
            TenantId = tenantId
        };

        await _workOrderExecCommandService.Handle(command);
    }
}