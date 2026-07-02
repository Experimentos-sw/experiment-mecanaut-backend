using AwawaTech.Mecanaut.API.Shared.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.ValueObjects;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Aggregates;

public class MaintenancePlanTemplate : AuditableAggregateRoot
{
    public string Name { get; private set; }
    public long MetricId { get; private set; }
    public double Amount { get; private set; }
    public long ProductionLineId { get; private set; }
    public long PlantLineId { get; private set; }
    public TenantId TenantId { get; private set; }
    public PlanStatus Status { get; private set; }

    protected MaintenancePlanTemplate() { }

    private MaintenancePlanTemplate(string name, long metricId, double amount, long productionLineId, long plantLineId, TenantId tenantId)
    {
        Name = name;
        MetricId = metricId;
        Amount = amount;
        ProductionLineId = productionLineId;
        PlantLineId = plantLineId;
        TenantId = tenantId;
        Status = PlanStatus.Active;
    }

    public static MaintenancePlanTemplate Create(string name, long metricId, double amount, long productionLineId, long plantLineId, TenantId tenantId)
    {
        return new MaintenancePlanTemplate(name, metricId, amount, productionLineId, plantLineId, tenantId);
    }
}
