using AwawaTech.Mecanaut.API.Shared.Domain.Model.Entities;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Entities;

public class MaintenancePlanTemplateMachine : AuditableEntity
{
    public long TemplateId { get; private set; }
    public long MachineId { get; private set; }

    protected MaintenancePlanTemplateMachine() { }

    public MaintenancePlanTemplateMachine(long templateId, long machineId)
    {
        TemplateId = templateId;
        MachineId = machineId;
    }
}
