using System.Collections.Generic;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Interfaces.REST.Resources;

public class MaintenancePlanTemplateResource
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MetricId { get; set; } = string.Empty;
    public double Amount { get; set; }
    public string ProductionLineId { get; set; } = string.Empty;
    public string PlantLineId { get; set; } = string.Empty;
    public List<long> Machines { get; set; } = new();
    public List<string> Tasks { get; set; } = new();
}
