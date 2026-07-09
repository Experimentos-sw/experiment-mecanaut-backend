using System.Collections.Generic;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates;

namespace AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects;

public class StartWorkOrderResult
{
    public bool Success { get; set; }
    public WorkOrder? WorkOrder { get; set; }
    public List<MissingPartInfo> MissingParts { get; set; } = new();
}

public class MissingPartInfo
{
    public long InventoryPartId { get; set; }
    public string PartNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int RequiredQuantity { get; set; }
    public int AvailableQuantity { get; set; }
}
