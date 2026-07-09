using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;

namespace AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Commands;

public class StartWorkOrderCommand
{
    public long WorkOrderId { get; set; }
    public TenantId TenantId { get; set; }
}
