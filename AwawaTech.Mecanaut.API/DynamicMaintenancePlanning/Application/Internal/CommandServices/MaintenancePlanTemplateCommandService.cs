using System;
using System.Collections.Generic;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Entities;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Repositories;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Services;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.Shared.Domain.Repositories;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Multitenancy;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Application.Internal.CommandServices;

public class MaintenancePlanTemplateCommandService : IMaintenancePlanTemplateCommandService
{
    private readonly IMaintenancePlanTemplateRepository templateRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly TenantContextHelper tenantHelper;

    private static readonly (string Name, long MetricId, double Amount, long ProductionLineId, long PlantLineId, long[] Machines, string[] Tasks)[] DEFAULT_TEMPLATES =
    {
        (
            "Template Básica de Mantenimiento de Línea",
            1,
            10.0,
            1,
            1,
            new long[] { 101, 102 },
            new[] { "Inspeccionar lubricación", "Revisar filtros" }
        ),
        (
            "Template de Revisión de Maquinaria",
            2,
            5.0,
            1,
            1,
            new long[] { 103 },
            new[] { "Verificar correas", "Ajustar tensión" }
        )
    };

    public MaintenancePlanTemplateCommandService(
        IMaintenancePlanTemplateRepository repo,
        IUnitOfWork uow,
        TenantContextHelper helper)
    {
        templateRepository = repo;
        unitOfWork = uow;
        tenantHelper = helper;
    }

    public async Task<MaintenancePlanTemplate> CreateAsync(CreateMaintenancePlanTemplateCommand command)
    {
        var tenantId = tenantHelper.GetCurrentTenantId();

        if (await templateRepository.ExistsByNameAsync(command.Name, tenantId))
            throw new InvalidOperationException("A maintenance plan template with this name already exists.");

        var template = MaintenancePlanTemplate.Create(
            command.Name,
            long.Parse(command.MetricId),
            command.Amount,
            long.Parse(command.ProductionLineId),
            long.Parse(command.PlantLineId),
            new TenantId(tenantId));

        await templateRepository.AddAsync(template);
        await unitOfWork.CompleteAsync();

        if (template.Id <= 0)
            throw new InvalidOperationException("The template ID was not generated correctly.");

        foreach (var machineId in command.Machines)
        {
            var entity = new MaintenancePlanTemplateMachine(template.Id, machineId);
            await templateRepository.AddEntityAsync(entity);
        }

        foreach (var taskDescription in command.Tasks)
        {
            var entity = new MaintenancePlanTemplateTask(template.Id, taskDescription);
            await templateRepository.AddEntityAsync(entity);
        }

        await unitOfWork.CompleteAsync();

        return template;
    }

    public async Task HandleAsync(SeedMaintenancePlanTemplatesCommand command)
    {
        var tenantId = tenantHelper.GetCurrentTenantId();

        foreach (var templateInfo in DEFAULT_TEMPLATES)
        {
            if (!await templateRepository.ExistsByNameAsync(templateInfo.Name, tenantId))
            {
                var template = MaintenancePlanTemplate.Create(
                    templateInfo.Name,
                    templateInfo.MetricId,
                    templateInfo.Amount,
                    templateInfo.ProductionLineId,
                    templateInfo.PlantLineId,
                    new TenantId(tenantId));

                await templateRepository.AddAsync(template);
                await unitOfWork.CompleteAsync();

                foreach (var machineId in templateInfo.Machines)
                {
                    await templateRepository.AddEntityAsync(new MaintenancePlanTemplateMachine(template.Id, machineId));
                }

                foreach (var task in templateInfo.Tasks)
                {
                    await templateRepository.AddEntityAsync(new MaintenancePlanTemplateTask(template.Id, task));
                }

                await unitOfWork.CompleteAsync();
            }
        }
    }
}
