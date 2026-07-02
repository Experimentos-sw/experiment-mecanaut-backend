using System.Collections.Generic;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Interfaces.REST.Resources;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Interfaces.REST.Transform;

public class CreateMaintenancePlanTemplateCommandFromResourceAssembler
{
    public CreateMaintenancePlanTemplateCommand ToCommand(CreateMaintenancePlanTemplateResource resource)
    {
        return new CreateMaintenancePlanTemplateCommand
        {
            Name = resource.Name,
            MetricId = resource.MetricId,
            Amount = resource.Amount,
            ProductionLineId = resource.ProductionLineId,
            PlantLineId = resource.PlantLineId,
            Machines = resource.Machines ?? new List<long>(),
            Tasks = resource.Tasks ?? new List<string>()
        };
    }
}
