using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Aggregates;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Services;

public interface IMaintenancePlanTemplateCommandService
{
    Task<MaintenancePlanTemplate> CreateAsync(CreateMaintenancePlanTemplateCommand command);
    Task HandleAsync(SeedMaintenancePlanTemplatesCommand command);
}
