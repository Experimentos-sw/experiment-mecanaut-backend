using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Repositories;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Repositories;

using Microsoft.EntityFrameworkCore;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Infrastructure.Persistence.EFC.Repositories;

public class DynamicMaintenancePlanRepository : BaseRepository<DynamicMaintenancePlan>, IDynamicMaintenancePlanRepository
{
    private readonly AppDbContext _context;

    public DynamicMaintenancePlanRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByNameAsync(string name, long tenantId)
        => await _context.Set<DynamicMaintenancePlan>().AnyAsync(p => p.Name == name && p.TenantId == new TenantId(tenantId));

    public async Task<DynamicMaintenancePlanWithDetails?> GetByIdAsync(string id, string tenantId)
    {
        if (!long.TryParse(id, out var planId))
            return null;

        var plan = await _context.Set<DynamicMaintenancePlan>()
            .FirstOrDefaultAsync(p => p.Id == planId && p.TenantId == new TenantId(long.Parse(tenantId)));

        if (plan == null)
            return null;

        var machines = await _context.Set<DynamicMaintenancePlanMachine>()
            .Where(m => m.PlanId == planId)
            .ToListAsync();

        var tasks = await _context.Set<DynamicMaintenancePlanTask>()
            .Where(t => t.PlanId == planId)
            .ToListAsync();

        return new DynamicMaintenancePlanWithDetails
        {
            Plan = plan,
            Machines = machines,
            Tasks = tasks
        };
    }


    /*
    public async Task<IEnumerable<DynamicMaintenancePlan>> GetAllByTenantIdAndPlantLineIdAsync(string tenantId, string plantLineId)
    {
        return await _context.Set<DynamicMaintenancePlan>()
            .Where(p => p.TenantId == new TenantId(long.Parse(tenantId)) 
                        && p.PlantLineId == (long.Parse(plantLineId))) // Filtro por PlantLineId
            .ToListAsync();
    }
    */
    
    public async Task<IEnumerable<DynamicMaintenancePlanWithDetails>> GetAllByTenantIdAndPlantLineIdAsync(string tenantId, string plantLineId)
    {
        var tenantLongId = long.Parse(tenantId);
        var plantLineLongId = long.Parse(plantLineId);

        var plans = await _context.Set<DynamicMaintenancePlan>()
            .Where(p => p.TenantId == new TenantId(tenantLongId) && p.PlantLineId == plantLineLongId)
            .ToListAsync();

        // Crear la lista de DynamicMaintenancePlanWithDetails para combinar los datos
        var result = new List<DynamicMaintenancePlanWithDetails>();

        foreach (var plan in plans)
        {
            var planDetails = new DynamicMaintenancePlanWithDetails
            {
                Plan = plan, // Agregar el plan base
                Machines = await _context.Set<DynamicMaintenancePlanMachine>()
                    .Where(m => m.PlanId == plan.Id)
                    .ToListAsync(), // Obtener las máquinas asociadas
                Tasks = await _context.Set<DynamicMaintenancePlanTask>()
                    .Where(t => t.PlanId == plan.Id)
                    .ToListAsync() // Obtener las tareas asociadas
            };

            result.Add(planDetails);
        }

        return result;
    }


    
    public async Task AddEntityAsync<T>(T entity) where T : class
    {
        await _context.Set<T>().AddAsync(entity);
    }
    
    public async Task<long?> GetPlanIdByMachineMetricAndAmountAsync(long machineId, long metricId, double amount)
    {
        return await _context.Set<DynamicMaintenancePlan>()
            .Join(
                _context.Set<DynamicMaintenancePlanMachine>(),
                plan => plan.Id,
                machine => machine.PlanId,
                (plan, machine) => new { plan, machine }
            )
            .Where(joined =>
                joined.machine.MachineId == machineId &&
                joined.plan.MetricId == metricId &&
                joined.plan.Amount <= amount
            )
            .Select(joined => joined.plan.Id)
            .FirstOrDefaultAsync();
    }

}