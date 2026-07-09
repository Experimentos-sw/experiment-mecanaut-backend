using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects;
namespace AwawaTech.Mecanaut.API.WorkOrders.Infrastructure.Persistence.EFC.Repositories;

public class WorkOrderRepository : BaseRepository<WorkOrder>, IWorkOrderRepository
{
    public WorkOrderRepository(AppDbContext context) : base(context){}

    public async Task<WorkOrder> FindByIdAsync(long id, TenantId tenantId)
    {
        return await Context.WorkOrders
            .Include(x => x.RequiredParts)
            .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);
    }

    public async Task<IEnumerable<WorkOrder>> FindByProductionLineAsync(long productionLineId, TenantId tenantId)
    {
        return await Context.WorkOrders
            .Where(x => x.ProductionLineId == productionLineId && x.TenantId == tenantId
            && x.Status == WorkOrderStatus.Pending)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<WorkOrder>> FindByProductionLineToExecuteAsync(long productionLineId, TenantId tenantId)
    {
        // 1️⃣ Ejecuta query SIN tocar la propiedad de conversión
        var list = await Context.WorkOrders
            .Where(x =>
                x.ProductionLineId == productionLineId &&
                x.TenantId == tenantId &&
                (x.Status == WorkOrderStatus.Pending || x.Status == WorkOrderStatus.InProgress))
            .AsNoTracking()
            .ToListAsync();

        // 2️⃣ Filtra en memoria usando la propiedad que es lista
        return list.Where(x => x.TechnicianIds != null && x.TechnicianIds.Any());
    }



} 