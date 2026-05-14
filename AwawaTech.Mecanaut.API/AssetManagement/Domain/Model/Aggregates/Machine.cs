using AwawaTech.Mecanaut.API.AssetManagement.Domain.Exceptions;
using AwawaTech.Mecanaut.API.AssetManagement.Domain.Model.Events;
using AwawaTech.Mecanaut.API.AssetManagement.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;

namespace AwawaTech.Mecanaut.API.AssetManagement.Domain.Model.Aggregates;

using MachineStatusEnum = AwawaTech.Mecanaut.API.AssetManagement.Domain.Model.ValueObjects.MachineStatus;

public class Machine : AuditableAggregateRoot
{
    public string SerialNumber { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public MachineSpecs Specs { get; private set; }
    public MachineStatusEnum Status { get; private set; }
    public MaintenanceInfo MaintenanceInfo { get; private set; }
    
    public long PlantId { get; private set; }
    
    public long? ProductionLineId { get; private set; }
    public TenantId TenantId { get; private set; }

    private Machine(string serial, string name, MachineSpecs specs, long plantId, TenantId tenantId)
    {
        ValidateSerial(serial);
        ValidateName(name);
        SerialNumber  = serial;
        Name          = name;
        Specs         = specs;
        PlantId        = plantId;
        TenantId = tenantId;
        Status        = MachineStatusEnum.Operational;
        MaintenanceInfo = MaintenanceInfo.CreateNew();
    }

    protected Machine() { }

    public static Machine Create(string serial, string name, MachineSpecs specs, long plantId, TenantId tenantId)
        => new(serial, name, specs, plantId, tenantId);

    /* ---- Behaviour ---- */
    public void AssignToProductionLine(long productionLineId)
    {
        if (!IsOperational()) throw new MachineNotOperationalException();
        if (Status == MachineStatusEnum.InMaintenance) throw new MachineInMaintenanceException();
        ProductionLineId = productionLineId;
        AddDomainEvent(new MachineAssignedEvent(Id, productionLineId));
    }

    public void StartMaintenance()
    {
        if (Status == MachineStatusEnum.InMaintenance) throw new MachineInMaintenanceException("Already in maintenance");
        var prev = Status;
        Status = MachineStatusEnum.InMaintenance;
        MaintenanceInfo = MaintenanceInfo.WithLastMaintenanceDate(DateTime.UtcNow);
        AddDomainEvent(new MachineMaintenanceStartedEvent(Id, prev));
    }

    public void CompleteMaintenance()
    {
        if (Status != MachineStatusEnum.InMaintenance) throw new InvalidOperationException("Not in maintenance");
        Status = MachineStatusEnum.Operational;
        MaintenanceInfo = MaintenanceInfo.WithNextMaintenanceDate();
        AddDomainEvent(new MachineMaintenanceCompletedEvent(Id));
    }

    public void RegisterMetric(string name, double value, double maxSafeThreshold)
    {
        if (value > maxSafeThreshold)
        {
            if (Status != MachineStatusEnum.InMaintenance)
            {
                var prev = Status;
                Status = MachineStatusEnum.InMaintenance;
                MaintenanceInfo = MaintenanceInfo.WithLastMaintenanceDate(DateTime.UtcNow);
                AddDomainEvent(new MachineMaintenanceStartedEvent(Id, prev));
            }
        }
    }

    /* ---- Queries ---- */
    public bool NeedsMaintenance() => MaintenanceInfo.IsMaintenanceDue();
    public bool IsOperational() => Status == MachineStatusEnum.Operational;
    public bool IsAvailableForAssignment() => IsOperational() && ProductionLineId == null;

    /* ---- Validation ---- */
    private static void ValidateSerial(string serial)
    {
        if (string.IsNullOrWhiteSpace(serial)) throw new ArgumentException("Serial required");
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");
    }
} 