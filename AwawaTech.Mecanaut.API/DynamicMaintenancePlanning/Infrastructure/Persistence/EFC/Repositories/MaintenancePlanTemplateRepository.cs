using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Repositories;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Infrastructure.Persistence.EFC.Repositories;

public class MaintenancePlanTemplateRepository : BaseRepository<MaintenancePlanTemplate>, IMaintenancePlanTemplateRepository
{
    private readonly AppDbContext _context;

    public MaintenancePlanTemplateRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByNameAsync(string name, long tenantId)
        => await _context.Set<MaintenancePlanTemplate>().AnyAsync(p => p.Name == name && p.TenantId == new TenantId(tenantId));

    public async Task<IEnumerable<MaintenancePlanTemplateWithDetails>> GetAllByTenantIdAsync(long tenantId)
    {
        var templates = await _context.Set<MaintenancePlanTemplate>()
            .Where(p => p.TenantId == new TenantId(tenantId))
            .ToListAsync();

        var result = new List<MaintenancePlanTemplateWithDetails>();

        foreach (var template in templates)
        {
            var machines = await _context.Set<MaintenancePlanTemplateMachine>()
                .Where(m => m.TemplateId == template.Id)
                .ToListAsync();

            var tasks = await _context.Set<MaintenancePlanTemplateTask>()
                .Where(t => t.TemplateId == template.Id)
                .ToListAsync();

            result.Add(new MaintenancePlanTemplateWithDetails
            {
                Template = template,
                Machines = machines,
                Tasks = tasks
            });
        }

        return result;
    }

    public async Task AddEntityAsync<T>(T entity) where T : class
    {
        await _context.Set<T>().AddAsync(entity);
    }
}
