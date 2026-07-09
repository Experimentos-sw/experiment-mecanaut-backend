namespace AwawaTech.Mecanaut.API.WorkOrders.Interfaces.REST.Resources;

public record StockVerificationResultResource(
    long InventoryPartId,
    string PartNumber,
    string Name,
    int RequiredQuantity,
    int AvailableQuantity,
    bool HasSufficientStock
);
