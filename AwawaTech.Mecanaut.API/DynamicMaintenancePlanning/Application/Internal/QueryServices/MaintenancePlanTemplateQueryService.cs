using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Repositories;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Services;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Multitenancy;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Application.Internal.QueryServices;

public class MaintenancePlanTemplateQueryService : IMaintenancePlanTemplateQueryService
{
    private readonly IMaintenancePlanTemplateRepository templateRepository;
    private readonly TenantContextHelper tenantHelper;

    public MaintenancePlanTemplateQueryService(
        IMaintenancePlanTemplateRepository repo,
        TenantContextHelper helper)
    {
        templateRepository = repo;
        tenantHelper = helper;
    }

    public async Task<IEnumerable<MaintenancePlanTemplateWithDetails>> GetAllAsync()
    {
        var tenantId = tenantHelper.GetCurrentTenantId();
        return await templateRepository.GetAllByTenantIdAsync(tenantId);
    }

    public async Task<MaintenancePlanTemplateWithDetails?> GetByIdAsync(long id)
    {
        var tenantId = tenantHelper.GetCurrentTenantId();
        return await templateRepository.GetByIdByTenantIdAsync(id, tenantId);
    }
}
