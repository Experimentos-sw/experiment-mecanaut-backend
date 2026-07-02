using System;
using System.Collections.Generic;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;

namespace AwawaTech.Mecanaut.API.ExecutedWorkOrders.Domain.Model.Commands;

public class CreateExecutedWorkOrderCommand
{
    public string Code { get; init; }
    public string Annotations { get; init; }
    public DateTime ExecutionDate { get; init; }
    public long ProductionLineId { get; init; }
    public List<long> IntervenedMachineIds { get; init; }
    public List<long?> AssignedTechnicianIds { get; init; }
    public List<string> ExecutedTasks { get; init; }
    public List<(long ProductId, int Quantity)> UsedProducts { get; init; }
    public List<string> ExecutionImages { get; init; }
    
    public bool IsAreaCleaned { get; init; }
    public bool AreToolsReturned { get; init; }
    public bool IsOperationsVerified { get; init; }
} 