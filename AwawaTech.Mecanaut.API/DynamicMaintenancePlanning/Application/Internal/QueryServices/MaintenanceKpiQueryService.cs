using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Services;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Application.Internal.QueryServices;

public class MaintenanceKpiQueryService : IMaintenanceKpiQueryService
{
    public Task<MaintenanceKpiProjection> ProjectKpisAsync(IEnumerable<long> machineIds, int metricId, double amount, int taskCount)
    {
        var machineCount = machineIds.Count();
        if (machineCount == 0) return Task.FromResult(new MaintenanceKpiProjection(0, 0));

        // Proyección determinística para simular cálculo basado en histórico
        var mtbf = 500.0 + (metricId * 12.5) + (amount * 0.05) - (machineCount * 8.0);
        var mttr = 4.0 + (machineCount * 1.2) - (amount * 0.001) + (taskCount * 1.5);

        // Limites de seguridad
        mtbf = Math.Max(24.0, mtbf); 
        mttr = Math.Max(1.0, mttr);

        return Task.FromResult(new MaintenanceKpiProjection(Math.Round(mtbf, 1), Math.Round(mttr, 1)));
    }
}
