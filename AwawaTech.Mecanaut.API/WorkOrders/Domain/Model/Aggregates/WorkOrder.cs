using AwawaTech.Mecanaut.API.Shared.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Events;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects;
namespace AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates;

public class WorkOrder : AuditableAggregateRoot
{
    public string Code { get; private set; }
    public TenantId TenantId { get; private set; }
    public List<long> MachineIds { get; private set; }
    public List<long?> TechnicianIds { get; private set; }
    public List<string> Tasks { get; private set; }
    public WorkOrderStatus Status { get; private set; }
    public DateTime Date { get; private set; }
    public long ProductionLineId { get; private set; }
    public WorkOrderType Type { get; private set; }
    public DateTime? ClosingDate { get; private set; }

    protected WorkOrder()
    {
        MachineIds = new List<long>();
        TechnicianIds = new List<long?>();
        Tasks = new List<string>();
    }

    private WorkOrder(
        string code,
        TenantId tenantId,
        DateTime date,
        long productionLineId,
        WorkOrderType type) : this()
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Work order code cannot be empty");

        Code = code;
        TenantId = tenantId;
        Date = date;
        ProductionLineId = productionLineId;
        Type = type;
        Status = WorkOrderStatus.Pending;

        AddDomainEvent(new WorkOrderCreatedEvent(Id, tenantId.Value, code));
    }

    public static WorkOrder Create(
        string code,
        TenantId tenantId,
        DateTime date,
        long productionLineId,
        WorkOrderType type)
    {
        return new WorkOrder(code, tenantId, date, productionLineId, type);
    }

    public void AssignMachines(IEnumerable<long> machineIds)
    {
        if (!machineIds.Any())
            throw new ArgumentException("At least one machine must be assigned");

        MachineIds.Clear();
        MachineIds.AddRange(machineIds);

        AddDomainEvent(new MachinesAssignedToWorkOrderEvent(Id, TenantId.Value, machineIds.ToList()));
    }

    public void AddTechnicians(IEnumerable<long?> technicianIds)
    {
        TechnicianIds.Clear();
        TechnicianIds.AddRange(technicianIds);

        AddDomainEvent(new TechniciansAddedToWorkOrderEvent(Id, TenantId.Value, technicianIds.ToList()));
    }

    public void AddTasks(IEnumerable<string> tasks)
    {
        if (!tasks.Any())
            throw new ArgumentException("At least one task must be added");

        Tasks.Clear();
        Tasks.AddRange(tasks);

        AddDomainEvent(new TasksAddedToWorkOrderEvent(Id, TenantId.Value, tasks.ToList()));
    }

    public void Complete()
    {
        if (Status == WorkOrderStatus.Completed)
            throw new Exception("Work order is already completed");

        if (!MachineIds.Any())
            throw new ArgumentException("Cannot complete work order without assigned machines");

        if (!Tasks.Any())
            throw new ArgumentException("Cannot complete work order without tasks");

        Status = WorkOrderStatus.Completed;
        ClosingDate = DateTime.UtcNow;
        AddDomainEvent(new WorkOrderCompletedEvent(Id, TenantId.Value));
    }

    public void Start()
    {
        if (Status != WorkOrderStatus.Pending)
            throw new ArgumentException("Work order can only be started when pending");

        Status = WorkOrderStatus.InProgress;
    }
} 