using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;

namespace AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Commands;

public class CompleteWorkOrderCommand
{
    public long WorkOrderId { get; set; }
    public TenantId TenantId { get; set; }
    public bool IsAreaCleaned { get; set; }
    public bool AreToolsReturned { get; set; }
    public bool IsOperationsVerified { get; set; }
} 