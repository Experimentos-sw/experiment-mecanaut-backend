using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Services;
using Microsoft.Extensions.Hosting;

namespace AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Application.Internal.EventHandlers;

public class MaintenancePlanTemplatesSeedHostedService(IServiceProvider services,
                                                     ILogger<MaintenancePlanTemplatesSeedHostedService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = services.CreateScope();
        var svc = scope.ServiceProvider.GetRequiredService<IMaintenancePlanTemplateCommandService>();

        logger.LogInformation("Seeding default maintenance plan templates...");
        await svc.HandleAsync(new SeedMaintenancePlanTemplatesCommand());
        logger.LogInformation("Maintenance plan templates seeding finished.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
