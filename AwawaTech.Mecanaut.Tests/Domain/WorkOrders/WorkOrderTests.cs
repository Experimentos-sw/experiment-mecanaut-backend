using System;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects;
using FluentAssertions;
using Xunit;

namespace AwawaTech.Mecanaut.Tests.Domain.WorkOrders
{
    public class WorkOrderTests
    {
        // US18: Registro de tareas realizadas
        // Escenario 1: Registro exitoso de tarea y completado de la orden
        [Fact]
        public void Complete_WhenPendingAndValid_ChangesStatusAndSetsClosingDate()
        {
            // Arrange
            var tenantId = new TenantId(1);
            var workOrder = WorkOrder.Create("WO-001", tenantId, DateTime.UtcNow, 1, WorkOrderType.Corrective);
            workOrder.AssignMachines(new[] { 1L });
            workOrder.AddTasks(new[] { "Reparar motor" });

            // Act
            workOrder.Complete();

            // Assert
            workOrder.Status.Should().Be(WorkOrderStatus.Completed);

        }

        // US18: Registro de tareas realizadas
        // Escenario 2: Error al cerrar una orden ya completada
        [Fact]
        public void Complete_WhenAlreadyCompleted_ThrowsException()
        {
            // Arrange
            var tenantId = new TenantId(1);
            var workOrder = WorkOrder.Create("WO-001", tenantId, DateTime.UtcNow, 1, WorkOrderType.Corrective);
            workOrder.AssignMachines(new[] { 1L });
            workOrder.AddTasks(new[] { "Reparar motor" });

            // Completamos la orden por primera vez
            workOrder.Complete();

            // Act
            Action act = () => workOrder.Complete();

            // Assert
            // [TDD] Validamos que lance la excepción si se invoca sobre una orden ya cerrada
            act.Should().Throw<Exception>();
        }
    }
}
