using System.Collections.Generic;
using System.Threading.Tasks;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.ValueObjects;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Services;

public interface IMaintenanceKpiQueryService
{
    Task<MaintenanceKpiProjection> ProjectKpisAsync(IEnumerable<long> machineIds, int metricId, double amount, int taskCount);
}
