using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Entities;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Services;

public interface IMaintenancePlanTemplateQueryService
{
    Task<IEnumerable<MaintenancePlanTemplateWithDetails>> GetAllAsync();
    Task<MaintenancePlanTemplateWithDetails?> GetByIdAsync(long id);
}
