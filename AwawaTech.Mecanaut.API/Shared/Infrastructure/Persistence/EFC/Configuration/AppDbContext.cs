using AwawaTech.Mecanaut.API.IAM.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.IAM.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Multitenancy;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.AssetManagement.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.AssetManagement.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.Competency.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.Competency.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.ConditionMonitoring.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.ConditionMonitoring.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using AwawaTech.Mecanaut.API.Subscription.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.Subscription.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects;

using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.FormContact.Domain.Model;

namespace AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Application database context for the Learning Center Platform
/// </summary>
/// <param name="options">
///     The options for the database context
/// </param>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
   public long CurrentTenantId => TenantContext.CurrentTenantId;

   /// <summary>
   ///     On configuring the database context
   /// </summary>
   /// <remarks>
   ///     This method is used to configure the database context.
   ///     It also adds the created and updated date interceptor to the database context.
   /// </remarks>
   /// <param name="builder">
   ///     The option builder for the database context
   /// </param>
   protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

   /// <summary>
   ///     On creating the database model
   /// </summary>
   /// <remarks>
   ///     This method is used to create the database model for the application.
   /// </remarks>
   /// <param name="builder">
   ///     The model builder for the database context
   /// </param>
   protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // -------------------- IAM Context --------------------

        // Roles
        builder.Entity<Role>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
            e.Property(r => r.Name).IsRequired();
        });

        // Tenants
        builder.Entity<Tenant>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
            e.Property(t => t.Ruc).IsRequired();
            e.Property(t => t.LegalName).IsRequired();
            e.Property(t => t.Code).IsRequired();
            
            // Email (value object as string)
            e.Property(t => t.Email)
             .HasConversion(
                 v => v.HasValue ? v.Value.Value : null,
                 v => string.IsNullOrEmpty(v) ? null : new EmailAddress(v!)
             )
             .HasColumnName("email")
             .HasMaxLength(255);

            // PhoneNumber (value object as string)
            e.Property(t => t.PhoneNumber)
             .HasConversion(
                 v => v.HasValue ? v.Value.Value : null,
                 v => string.IsNullOrEmpty(v) ? null : new PhoneNumber(v!)
             )
             .HasColumnName("phone_number")
             .HasMaxLength(25);
            
            e.Property(t => t.SubscriptionPlanId).IsRequired();
        });

        // Users
        builder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
            e.Property(u => u.Username).IsRequired();
            e.Property(u => u.PasswordHash).IsRequired();

            e.Property(u => u.Email)
             .HasConversion(
                 v => v.HasValue ? v.Value.Value : null,
                 v => string.IsNullOrEmpty(v) ? null : new EmailAddress(v!)
             )
             .HasColumnName("email")
             .HasMaxLength(255);

            e.HasMany(u => u.Roles)
             .WithMany()
             .UsingEntity(j => j.ToTable("user_roles"));

            e.HasQueryFilter(u => u.TenantId == CurrentTenantId);
        });

        // ------------------ AssetManagement ------------------
        builder.Entity<Plant>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id)
             .ValueGeneratedOnAdd()
             .HasColumnName("plant_id");
            e.Property(p => p.Name).IsRequired();

            // Tenant filter (manual when querying)

            // Value Objects
            e.OwnsOne(p => p.Location, l =>
            {
                l.Property(v => v.Address).HasColumnName("address");
                l.Property(v => v.City).HasColumnName("city");
                l.Property(v => v.Country).HasColumnName("country");
            });
            e.OwnsOne(p => p.ContactInfo, c =>
            {
                c.Property(v => v.Phone).HasColumnName("phone");
                c.Property(v => v.Email).HasColumnName("email");
            });

            e.Property(p => p.TenantId)
             .HasConversion(v => v.Value,
                            v => new TenantId(v))
             .HasColumnName("tenant_id");
        });

        builder.Entity<ProductionLine>(e =>
        {
            e.HasKey(pl => pl.Id);
            e.Property(pl => pl.Id)
             .ValueGeneratedOnAdd()
             .HasColumnName("production_line_id");
            e.Property(pl => pl.Name).IsRequired();
            e.Property(pl => pl.Code).IsRequired();

            // Capacity value object
            e.OwnsOne(pl => pl.Capacity, c =>
            {
                c.Property(v => v.UnitsPerHour).HasColumnName("units_per_hour");
            });

            e.Property(pl => pl.TenantId)
             .HasConversion(v => v.Value,
                            v => new TenantId(v))
             .HasColumnName("tenant_id");
        });

        builder.Entity<Machine>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Id)
             .ValueGeneratedOnAdd()
             .HasColumnName("machine_id");
            e.Property(m => m.SerialNumber).IsRequired();
            e.Property(m => m.Name).IsRequired();
            e.Property(m => m.PlantId).IsRequired();
            e.HasIndex(m => m.SerialNumber).IsUnique();

            e.OwnsOne(m => m.Specs, s =>
            {
                s.Property(v => v.Manufacturer).HasColumnName("manufacturer");
                s.Property(v => v.Model).HasColumnName("model");
                s.Property(v => v.Type).HasColumnName("type");
                s.Property(v => v.PowerConsumption).HasColumnName("power_consumption");
            });
            e.OwnsOne(m => m.MaintenanceInfo, mi =>
            {
                mi.Property(v => v.LastMaintenance).HasColumnName("last_maintenance");
                mi.Property(v => v.NextMaintenance).HasColumnName("next_maintenance");
            });

            e.Property(m => m.TenantId)
             .HasConversion(v => v.Value,
                            v => new TenantId(v))
             .HasColumnName("tenant_id");
        });

        // ------------------ Competency ------------------
        builder.Entity<Skill>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Id)
             .ValueGeneratedOnAdd()
             .HasColumnName("skill_id");

            e.Property(s => s.Name).IsRequired().HasMaxLength(100);
            e.Property(s => s.Description).HasMaxLength(255);
            e.Property(s => s.Category).HasMaxLength(50);

            e.Property(s => s.Status)
             .HasConversion<string>()
             .HasColumnName("status");

            e.Property(s => s.TenantId)
             .HasConversion(v => v.Value, v => new TenantId(v))
             .HasColumnName("tenant_id");

            e.HasIndex(s => new { s.Name, s.TenantId }).IsUnique();
        });

        // ------------------ ConditionMonitoring ------------------
        builder.Entity<MetricDefinition>(e =>
        {
            e.HasKey(md => md.Id);
            e.Property(md => md.Id).ValueGeneratedOnAdd().HasColumnName("metric_definition_id");
            e.Property(md => md.Name).IsRequired().HasMaxLength(100);
            e.Property(md => md.Unit).IsRequired().HasMaxLength(20);
            e.HasIndex(md => md.Name).IsUnique();
        });

        builder.Entity<MachineMetrics>(e =>
        {
            e.HasKey(mm => mm.Id);
            e.Property(mm => mm.Id).ValueGeneratedOnAdd().HasColumnName("machine_metrics_id");
            e.Property(mm => mm.MachineId).HasColumnName("machine_id").IsRequired();
            e.Property(mm => mm.TenantId)
             .HasConversion(v => v.Value, v => new TenantId(v))
             .HasColumnName("tenant_id");
            e.HasIndex(mm => mm.MachineId).IsUnique();

            // CurrentReadings se calcula en dominio; no se persiste directamente
            e.Ignore(mm => mm.CurrentReadings);

            // readings 1-N
            e.HasMany(mm => mm.Readings).WithOne().HasForeignKey("machine_metrics_id");
        });

        builder.Entity<MetricReading>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.Id).ValueGeneratedOnAdd().HasColumnName("metric_reading_id");
            e.Property(r => r.MachineId).HasColumnName("machine_id");
            e.Property(r => r.MetricId).HasColumnName("metric_id");
            e.Property(r => r.Value).HasColumnName("value");
            e.Property(r => r.MeasuredAt).HasColumnName("measured_at");
            e.Property(r => r.TenantId).HasColumnName("tenant_id");
            e.HasIndex(r => new { r.MachineId, r.MetricId, r.MeasuredAt })
             .HasDatabaseName("idx_machine_metric_time");
        });
        
        
        // ------------------ SubscriptionPlan ------------------
        
        builder.Entity<SubscriptionPlan>(e =>
        {
            e.HasKey(sp => sp.Id);
            e.Property(sp => sp.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("subscription_plan_id"); // Puedes personalizar el nombre de la columna

            e.Property(sp => sp.Name).IsRequired();
            e.Property(sp => sp.Description).IsRequired();
            e.Property(sp => sp.Price).IsRequired();
            e.Property(sp => sp.Currency).IsRequired();

            // Configuración de los Value Objects
            e.OwnsOne(sp => sp.Features, f =>
            {
                f.Property(v => v.MaxMachines).HasColumnName("max_machines");
                f.Property(v => v.MaxUsers).HasColumnName("max_users");
                f.Property(v => v.SupportPriority).HasColumnName("support_priority");
                f.Property(v => v.PredictiveMaintenance).HasColumnName("predictive_maintenance");
                f.Property(v => v.AdvancedAnalytics).HasColumnName("advanced_analytics");
            });

            // Configuración del estado del plan
            e.Property(sp => sp.Status)
                .HasConversion(v => v.Value, v => SubscriptionStatus.FromString(v))
                .HasColumnName("status")
                .IsRequired();
        });

        // ------------------ SupportRequest ------------------
        builder.Entity<SupportRequest>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Id)
             .ValueGeneratedOnAdd()
             .HasColumnName("support_request_id");

            e.Property(s => s.Name)
             .IsRequired()
             .HasMaxLength(255)
             .HasColumnName("name");

            e.Property(s => s.Email)
             .IsRequired()
             .HasMaxLength(255)
             .HasColumnName("email");

            e.Property(s => s.Message)
             .IsRequired()
             .HasColumnName("message");

            e.Property(s => s.SubmittedAt)
             .IsRequired()
             .HasColumnName("submitted_at");
        });

        // ------------------ InventoryManagement ------------------
        builder.Entity<InventoryPart>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id)
             .ValueGeneratedOnAdd()
             .HasColumnName("inventory_part_id");
            
            e.Property(p => p.PartNumber).IsRequired();
            e.Property(p => p.Name).IsRequired();
            e.Property(p => p.Description);
            
            // Value Objects
            e.Property(p => p.CurrentStock).HasColumnName("current_stock");
            e.Property(p => p.MinimumStock).HasColumnName("min_stock");
            e.Property(p => p.PlantId).HasColumnName("plant_id");
            e.Property(p => p.UnitPrice)
                .HasConversion(
                    price => price.Amount,
                    amount => new Money(amount, "PEN"))
                .HasColumnName("unit_price");

            e.HasIndex(p => new { p.PartNumber }).IsUnique();
        });

        builder.Entity<PurchaseOrder>(e =>
        {
            e.HasKey(po => po.Id);
            e.Property(po => po.Id)
             .ValueGeneratedOnAdd()
             .HasColumnName("purchase_order_id");
            
            e.Property(po => po.OrderNumber).IsRequired();
            e.Property(po => po.Status)
             .HasConversion<string>()
             .HasColumnName("status");
            e.Property(po => po.OrderDate).HasColumnName("order_date");
            e.Property(po => po.DeliveryDate).HasColumnName("delivery_date");
            e.Property(po => po.TotalPrice)
                .HasConversion(
                    price => price.Amount,
                    amount => new Money(amount, "PEN"))
                .HasColumnName("total_price");
            e.Property(po => po.PlantId).HasColumnName("plant_id");
            
        });

        // ------------------ DynamicMaintenancePlanning ------------------
        builder.Entity<DynamicMaintenancePlan>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id)
             .ValueGeneratedOnAdd()
             .HasColumnName("dynamic_maintenance_plan_id");
            
            e.Property(p => p.Name)
             .IsRequired()
             .HasMaxLength(100);
            
            e.Property(p => p.MetricId)
             .IsRequired()
             .HasColumnName("metric_id");
            
            e.Property(p => p.Amount)
                .IsRequired()
                .HasColumnName("amount");

            e.Property(p => p.Status)
             .IsRequired()
             .HasConversion<string>()
             .HasColumnName("status");

            e.Property(p => p.TenantId)
             .HasConversion(v => v.Value, v => new TenantId(v))
             .HasColumnName("tenant_id");

            // Índice único para nombre por tenant
            e.HasIndex(p => new { p.Name, p.TenantId }).IsUnique();
        });

        builder.Entity<DynamicMaintenancePlanMachine>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("d_plan_machine_id");
    
            e.Property(pm => pm.PlanId)
                .HasColumnName("plan_id");
    
            e.Property(pm => pm.MachineId)
                .HasColumnName("machine_id");

            // Relación con el plan
            e.HasOne<DynamicMaintenancePlan>()
                .WithMany()
                .HasForeignKey(pm => pm.PlanId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DynamicMaintenancePlanMachine_Plan");  // Nombre corto para el constraint
        });


        builder.Entity<DynamicMaintenancePlanTask>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("d_plan_task_id");
    
            e.Property(pt => pt.PlanId)
                .HasColumnName("plan_id");
    
            e.Property(pt => pt.TaskDescription)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("task_description");
            
            e.HasOne<DynamicMaintenancePlan>()
                .WithMany()
                .HasForeignKey(pt => pt.PlanId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DynamicMaintenancePlanTask_Plan"); // Nombre corto para el constraint
        });

        builder.Entity<MaintenancePlanTemplate>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("maintenance_plan_template_id");

            e.Property(p => p.Name)
             .IsRequired()
             .HasMaxLength(100);

            e.Property(p => p.MetricId)
             .IsRequired()
             .HasColumnName("metric_id");

            e.Property(p => p.Amount)
                .IsRequired()
                .HasColumnName("amount");

            e.Property(p => p.ProductionLineId)
                .IsRequired()
                .HasColumnName("production_line_id");

            e.Property(p => p.PlantLineId)
                .IsRequired()
                .HasColumnName("plant_line_id");

            e.Property(p => p.TenantId)
             .HasConversion(v => v.Value, v => new TenantId(v))
             .HasColumnName("tenant_id");

            e.Property(p => p.Status)
             .IsRequired()
             .HasConversion<string>()
             .HasColumnName("status");

            e.HasIndex(p => new { p.Name, p.TenantId }).IsUnique();
        });

        builder.Entity<MaintenancePlanTemplateMachine>(e =>
        {
            e.HasKey(p => p.Id);

            e.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("maintenance_plan_template_machine_id");

            e.Property(pm => pm.TemplateId)
                .HasColumnName("maintenance_plan_template_id");

            e.Property(pm => pm.MachineId)
                .HasColumnName("machine_id");

            e.HasIndex(pm => pm.TemplateId)
                .HasDatabaseName("IX_MPTM_Template");

            e.HasIndex(pm => pm.MachineId)
                .HasDatabaseName("IX_MPTM_Machine");

            e.HasOne<MaintenancePlanTemplate>()
                .WithMany()
                .HasForeignKey(pm => pm.TemplateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_MaintenancePlanTemplateMachine_Template");
        });

        builder.Entity<MaintenancePlanTemplateTask>(e =>
        {
            e.HasKey(p => p.Id);

            e.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("maintenance_plan_template_task_id");

            e.Property(pt => pt.TemplateId)
                .HasColumnName("maintenance_plan_template_id");

            e.Property(pt => pt.TaskDescription)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("task_description");

            e.HasIndex(pt => pt.TemplateId)
                .HasDatabaseName("IX_MPTT_Template");

            e.HasOne<MaintenancePlanTemplate>()
                .WithMany()
                .HasForeignKey(pt => pt.TemplateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_MaintenancePlanTemplateTask_Template");
        });


        // -------------------- WorkOrders Context --------------------
        builder.Entity<WorkOrder>(e =>
        {
            e.HasKey(w => w.Id);
            e.Property(w => w.Id)
             .ValueGeneratedOnAdd()
             .HasColumnName("work_order_id");
            
            e.Property(w => w.Code).IsRequired();
            e.Property(w => w.Date).IsRequired();
            e.Property(w => w.ProductionLineId).IsRequired();
            e.Property(w => w.Status)
             .HasConversion<string>()
             .IsRequired();
            e.Property(w => w.Type)
             .HasConversion<string>()
             .IsRequired();

            // TenantId conversion
            e.Property(w => w.TenantId)
             .HasConversion(v => v.Value,
                          v => new TenantId(v))
             .HasColumnName("tenant_id");

            // Collections as JSON
            e.Property(w => w.MachineIds)
             .HasColumnName("machine_ids")
             .HasConversion(
                 v => string.Join(",", v),
                 v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList()
             );

            e.Property(w => w.TechnicianIds)
             .HasColumnName("technician_ids")
             .HasConversion(
                 v => string.Join(",", v.Select(id => id.HasValue ? id.Value.ToString() : "")),
                 v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => string.IsNullOrEmpty(s) ? (long?)null : long.Parse(s))
                      .ToList()
             );

            e.Property(w => w.Tasks)
             .HasColumnName("tasks")
             .HasConversion(
                 v => string.Join("|", v),
                 v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
             );
        });
        
        // ExecutedWorkOrder
        builder.Entity<ExecutedWorkOrder>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Code).IsRequired();
            e.Property(x => x.Annotations).IsRequired();
            e.Property(w => w.TenantId)
                .HasConversion(v => v.Value,
                    v => new TenantId(v))
                .HasColumnName("tenant_id");
            e.Property(x => x.ExecutionDate).IsRequired();
            e.Property(x => x.ProductionLineId).IsRequired();
    
            e.Property(x => x.IntervenedMachineIds)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(long.Parse)
                        .ToList());
                
            e.Property(x => x.AssignedTechnicianIds)
                .HasConversion(
                    v => string.Join(",", v.Select(id => id.HasValue ? id.Value.ToString() : "")),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => string.IsNullOrEmpty(s) ? (long?)null : long.Parse(s))
                        .ToList()
                );
                
            e.Property(x => x.ExecutedTasks)
                .HasConversion(
                    v => string.Join('|', v),
                    v => v.Split('|', StringSplitOptions.RemoveEmptyEntries)
                        .ToList());
        });

        // UsedProduct
        builder.Entity<UsedProduct>(e =>
        {
            e.HasKey(x => x.Id);
            //e.Property(x => x.Id).ValueGeneratedOnAdd();
            e.Property(x => x.ExecutedWorkOrderId).IsRequired();
            e.Property(x => x.ProductId).IsRequired();
            e.Property(x => x.Quantity).IsRequired();
    
            e.HasOne<ExecutedWorkOrder>()
                .WithMany()
                .HasForeignKey(x => x.ExecutedWorkOrderId);
        });
        
        builder.Entity<ExecutionImages>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.ExecutedWorkOrderId).IsRequired();
            e.Property(x => x.ImageUrl).IsRequired();
    
            e.HasOne<ExecutedWorkOrder>()
                .WithMany()
                .HasForeignKey(x => x.ExecutedWorkOrderId);
        });
        
        // ------------------ ExperimentSurveys ------------------
        builder.Entity<ExperimentSurvey>(e =>
        {
            e.ToTable("ExperimentSurveys");
            e.HasKey(x => x.Id);
            e.Property(x => x.Rating);
            e.Property(x => x.Variant);
            e.Property(x => x.Comment);
        });

        builder.UseSnakeCaseNamingConvention();
    }

   // DbSets for AssetManagement
   public DbSet<Plant> Plants { get; set; } = null!;
   public DbSet<ProductionLine> ProductionLines { get; set; } = null!;
   public DbSet<Machine> Machines { get; set; } = null!;
   public DbSet<Skill> Skills { get; set; } = null!;
   public DbSet<MachineMetrics> MachineMetrics { get; set; } = null!;
   public DbSet<MetricDefinition> MetricDefinitions { get; set; } = null!;
   public DbSet<MetricReading> MetricReadings { get; set; } = null!;
   public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; } = null!;
   public DbSet<InventoryPart> InventoryParts { get; set; } = null!;
   public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;

   // DbSet para DynamicMaintenancePlanning
   public DbSet<DynamicMaintenancePlan> DynamicMaintenancePlans { get; set; } = null!;
   public DbSet<DynamicMaintenancePlanMachine> DynamicMaintenancePlanMachines { get; set; } = null!;
   public DbSet<DynamicMaintenancePlanTask> DynamicMaintenancePlanTasks { get; set; } = null!;
   public DbSet<MaintenancePlanTemplate> MaintenancePlanTemplates { get; set; } = null!;
   public DbSet<MaintenancePlanTemplateMachine> MaintenancePlanTemplateMachines { get; set; } = null!;
   public DbSet<MaintenancePlanTemplateTask> MaintenancePlanTemplateTasks { get; set; } = null!;
   

   //DbSet para ExecutionWorkOrders
   
   public DbSet<ExecutedWorkOrder> ExecutedWorkOrders { get; set; } = null!;
   public DbSet<UsedProduct> UsedProducts { get; set; } = null!;
   public DbSet<ExecutionImages> ExecutionImages { get; set; } = null!;
   public DbSet<WorkOrder> WorkOrders { get; set; } = null!;
   
   //DbSet para ExperimentSurveys
   
   public DbSet<ExperimentSurvey> ExperimentSurveys { get; set; } = null!;
}
