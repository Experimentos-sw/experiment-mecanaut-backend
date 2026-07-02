using System.Collections.Generic;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Aggregates;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Entities;

public class MaintenancePlanTemplateWithDetails
{
    public MaintenancePlanTemplate Template { get; set; } = null!;
    public List<MaintenancePlanTemplateMachine> Machines { get; set; } = new();
    public List<MaintenancePlanTemplateTask> Tasks { get; set; } = new();
}
