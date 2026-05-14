using System;
using AwawaTech.Mecanaut.API.AssetManagement.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.AssetManagement.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;
using FluentAssertions;
using Xunit;

namespace AwawaTech.Mecanaut.Tests.Domain.AssetManagement
{
    public class MachineTests
    {
        // US23: Registro de métricas y evaluación de umbrales
        // Escenario 1: Registro exitoso y se mantiene el estado (Valor <= Umbral)
        [Fact]
        public void RegisterMetric_WithNormalValue_MaintainsOperationalStatus()
        {
            var specs = new MachineSpecs("Fabricante", "Modelo", "Manual", 100.0);
            var tenantId = new TenantId(1);
            var machine = Machine.Create("MAC-001", "Prensa", specs, 1, tenantId);
            
            var metricValue = 50.0;
            var maxSafeThreshold = 100.0;

            // Act
            // [TDD] Método a implementar en Machine
            machine.RegisterMetric("Vibracion", metricValue, maxSafeThreshold);

            // Assert
            machine.IsOperational().Should().BeTrue();
            machine.Status.Should().Be(MachineStatus.Operational);
        }

        // US23: Registro de métricas y evaluación de umbrales
        // Escenario 2: Valor supera el umbral máximo de seguridad
        [Fact]
        public void RegisterMetric_WithValueAboveThreshold_ChangesStatusToMaintenance()
        {
            // Arrange
            var specs = new MachineSpecs("Fabricante", "Modelo", "Manual", 100.0);
            var tenantId = new TenantId(1);
            var machine = Machine.Create("MAC-001", "Prensa", specs, 1, tenantId);
            
            var metricValue = 120.0;
            var maxSafeThreshold = 100.0;

            // Act
            machine.RegisterMetric("Vibracion", metricValue, maxSafeThreshold);

            // Assert
            // [TDD] Si el valor supera el umbral, el estado cambia automáticamente a mantenimiento (o alerta)
            machine.Status.Should().Be(MachineStatus.InMaintenance);
        }
    }
}
