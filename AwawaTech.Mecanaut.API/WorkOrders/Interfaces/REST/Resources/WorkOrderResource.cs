using System;
using System.Collections.Generic;

namespace AwawaTech.Mecanaut.API.WorkOrders.Interfaces.REST.Resources;

public record WorkOrderResource(
    long Id,
    string Code,
    string Status,
    string Type,
    DateTime Date,
    long ProductionLineId,
    List<long> MachineIds,
    List<long?> TechnicianIds,
    List<string> Tasks,
    List<RequiredPartResource> RequiredParts
); 