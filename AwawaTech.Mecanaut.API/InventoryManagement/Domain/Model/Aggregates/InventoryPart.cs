using System;
using AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.Events;
using AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.Entities;

namespace AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.Aggregates;

public class InventoryPart : AuditableAggregateRoot
{
    public string PartNumber { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public int CurrentStock { get; private set; }
    public int MinimumStock { get; private set; }
    public long PlantId { get; private set; }
    public Money UnitPrice { get; private set; }

    private InventoryPart(string partNumber, string name, string description, 
        int currentStock, int minimumStock, long plantId, Money unitPrice)
    {
        ValidatePartNumber(partNumber);
        ValidateName(name);
        ValidateStock(currentStock, minimumStock);
        
        PartNumber = partNumber;
        Name = name;
        Description = description;
        CurrentStock = currentStock;
        MinimumStock = minimumStock;
        PlantId = plantId;
        UnitPrice = unitPrice;
        
        AddDomainEvent(new InventoryPartCreatedEvent(Id, partNumber, plantId));
    }

    protected InventoryPart() { }

    public static InventoryPart Create(string partNumber, string name, string description,
        int currentStock, int minimumStock, long plantId, Money unitPrice)
        => new(partNumber, name, description, currentStock, minimumStock, plantId, unitPrice);

    /* ---- Behaviour ---- */
    public void UpdateStock(int quantity)
    {
        ValidateStock(quantity, MinimumStock);
        CurrentStock = quantity;
        AddDomainEvent(new InventoryPartStockUpdatedEvent(Id, quantity));
    }

    public void UpdateInfo(string name, string description, int minimumStock)
    {
        ValidateName(name);
        ValidateStock(CurrentStock, minimumStock);
        Name = name;
        Description = description;
        MinimumStock = minimumStock;
        AddDomainEvent(new InventoryPartUpdatedEvent(Id));
    }

    public void UpdatePrice(Money newPrice)
    {
        if (newPrice.Amount < 0)
            throw new ArgumentException("Price cannot be negative");
            
        UnitPrice = newPrice;
        AddDomainEvent(new InventoryPartUpdatedEvent(Id));
    }

    /* ---- Validation ---- */
    private static void ValidatePartNumber(string partNumber)
    {
        if (string.IsNullOrWhiteSpace(partNumber))
            throw new ArgumentException("Part number is required");
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");
    }

    private static void ValidateStock(int currentStock, int minimumStock)
    {
        if (currentStock < 0)
            throw new ArgumentException("Current stock cannot be negative");
        if (minimumStock < 0)
            throw new ArgumentException("Minimum stock cannot be negative");
    }

    public void DecreaseInventory(long quantity)
    {
        if (quantity > CurrentStock) throw new InvalidOperationException("No hay suficiente stock.");
        CurrentStock -= (int)quantity;
    }
} 