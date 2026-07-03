using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.Shared.Domain.Repositories;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Repositories;

public interface IMaintenancePlanTemplateRepository : IBaseRepository<MaintenancePlanTemplate>
{
    Task<bool> ExistsByNameAsync(string name, long tenantId);
    Task<IEnumerable<MaintenancePlanTemplateWithDetails>> GetAllByTenantIdAsync(long tenantId);
    Task<MaintenancePlanTemplateWithDetails?> GetByIdByTenantIdAsync(long id, long tenantId);
    Task AddEntityAsync<T>(T entity) where T : class;
}
