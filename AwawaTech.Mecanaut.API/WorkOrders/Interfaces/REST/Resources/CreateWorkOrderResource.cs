using System;
using System.Collections.Generic;

namespace AwawaTech.Mecanaut.API.WorkOrders.Interfaces.REST.Resources;

public record RequiredPartResource(long InventoryPartId, int Quantity);

public record CreateWorkOrderResource(
    string Code,
    DateTime Date,
    long ProductionLineId,
    string Type,
    List<long> MachineIds,
    List<string> Tasks,
    List<long?> TechnicianIds,
    List<RequiredPartResource>? RequiredParts = null
); 