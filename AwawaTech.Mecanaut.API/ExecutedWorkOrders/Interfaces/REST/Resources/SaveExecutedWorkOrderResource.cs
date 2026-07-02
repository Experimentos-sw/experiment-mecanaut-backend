using System;
using System.Collections.Generic;

namespace AwawaTech.Mecanaut.API.ExecutedWorkOrders.Interfaces.REST.Resources;

public class SaveExecutedWorkOrderResource
{
    public string Code { get; set; }
    public string Annotations { get; set; }
    public DateTime ExecutionDate { get; set; }
    public long ProductionLineId { get; set; }
    public List<long> IntervenedMachineIds { get; set; }
    public List<long?> AssignedTechnicianIds { get; set; }
    public List<string> ExecutedTasks { get; set; }
    public List<ProductQuantityResource> UsedProducts { get; set; }
    
    public List<string> Files { get; set; }
    
    public long WorkOrderId { get; set; }
}

public class ProductQuantityResource
{
    public long ProductId { get; set; }
    public int Quantity { get; set; }
} 