using System;
using System.Linq;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;
using FluentAssertions;
using Xunit;

namespace AwawaTech.Mecanaut.Tests.Domain.MaintenancePlanning
{
    public class MaintenancePlanTests
    {
        // US09: Implementación de plan de mantenimiento
        // Escenario: Generación de tareas futuras. Valida que calcule bien las fechas según una duración establecida.
        [Fact]
        public void GenerateFutureTasks_WithValidDuration_CalculatesDatesCorrectly()
        {
            // Arrange
            var tenantId = new TenantId(1);
            var plan = DynamicMaintenancePlan.Create("Plan Trimestral", 1, 100, 2, 1, tenantId);
            
            var startDate = new DateTime(2026, 1, 1);
            var durationInDays = 90; // Tarea recurrente cada 90 días
            var tasksToGenerate = 3;

            // Act
            // [TDD] Método a implementar en la entidad DynamicMaintenancePlan
            plan.GenerateFutureTasks(startDate, durationInDays, tasksToGenerate);

            // Assert
            plan.GeneratedTasks.Should().NotBeNull();
            plan.GeneratedTasks.Should().HaveCount(tasksToGenerate);
            
            // Validamos que se calculen bien las fechas iterativamente
            plan.GeneratedTasks.ElementAt(0).Date.Should().Be(startDate.AddDays(90));
            plan.GeneratedTasks.ElementAt(1).Date.Should().Be(startDate.AddDays(180));
            plan.GeneratedTasks.ElementAt(2).Date.Should().Be(startDate.AddDays(270));
        }
    }
}
