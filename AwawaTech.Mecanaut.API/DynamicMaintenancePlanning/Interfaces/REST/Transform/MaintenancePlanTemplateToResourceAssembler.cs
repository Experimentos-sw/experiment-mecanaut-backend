using System.Collections.Generic;
using System.Linq;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Interfaces.REST.Resources;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Interfaces.REST.Transform;

public class MaintenancePlanTemplateToResourceAssembler
{
    public MaintenancePlanTemplateResource ToResource(MaintenancePlanTemplateWithDetails templateWithDetails)
    {
        return new MaintenancePlanTemplateResource
        {
            Id = templateWithDetails.Template.Id,
            Name = templateWithDetails.Template.Name,
            MetricId = templateWithDetails.Template.MetricId.ToString(),
            Amount = templateWithDetails.Template.Amount,
            ProductionLineId = templateWithDetails.Template.ProductionLineId.ToString(),
            PlantLineId = templateWithDetails.Template.PlantLineId.ToString(),
            Machines = templateWithDetails.Machines.Select(m => m.MachineId).ToList(),
            Tasks = templateWithDetails.Tasks.Select(t => t.TaskDescription).ToList()
        };
    }
}
