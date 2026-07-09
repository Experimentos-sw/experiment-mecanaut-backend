namespace AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Entities;

public class WorkOrderRequiredPart
{
    public long Id { get; set; }
    public long WorkOrderId { get; set; }
    public long InventoryPartId { get; set; }
    public int Quantity { get; set; }

    public WorkOrderRequiredPart(long inventoryPartId, int quantity)
    {
        InventoryPartId = inventoryPartId;
        Quantity = quantity;
    }

    protected WorkOrderRequiredPart() { }
}
