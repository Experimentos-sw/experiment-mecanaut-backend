using AwawaTech.Mecanaut.API.Shared.Domain.Model.Entities;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Entities;

public class MaintenancePlanTemplateTask : AuditableEntity
{
    public long TemplateId { get; private set; }
    public string TaskDescription { get; private set; }

    protected MaintenancePlanTemplateTask() { }

    public MaintenancePlanTemplateTask(long templateId, string taskDescription)
    {
        TemplateId = templateId;
        TaskDescription = taskDescription;
    }
}
