using System;
using System.Collections.Generic;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects;

namespace AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Commands;

public record RequiredPartCommand(long InventoryPartId, int Quantity);

public class CreateWorkOrderCommand
{
    public string Code { get; set; }
    public TenantId TenantId { get; set; }
    public DateTime Date { get; set; }
    public long ProductionLineId { get; set; }
    public WorkOrderType Type { get; set; }
    public List<long> MachineIds { get; set; } = new();
    public List<string> Tasks { get; set; } = new();
    public List<long?> TechnicianIds { get; set; } = new();
    public List<RequiredPartCommand> RequiredParts { get; set; } = new();
} 