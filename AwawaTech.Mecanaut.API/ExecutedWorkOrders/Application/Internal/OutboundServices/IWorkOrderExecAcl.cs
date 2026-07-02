namespace AwawaTech.Mecanaut.API.ExecutedWorkOrders.Application.Internal.OutboundServices;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;

public interface IWorkOrderExecAcl
{
    Task MarkAsCompletedAsync(long workOrderId, TenantId tenantId);
}