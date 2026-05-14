using System;
using AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.ValueObjects;
using FluentAssertions;
using Xunit;

namespace AwawaTech.Mecanaut.Tests.Domain.InventoryManagement
{
    public class InventoryPartTests
    {
        // US31: Actualización de inventario
        // Escenario 1: Restar correctamente una cantidad válida
        [Fact]
        public void DecreaseStock_WithValidAmount_SubtractsCorrectly()
        {
            // Arrange
            var unitPrice = new Money(50.0m, "USD");
            var part = InventoryPart.Create("PN-001", "Filtro", "Filtro de aire", 100, 10, 1, unitPrice);
            var amountToDecrease = 20;

            // Act
            part.DecreaseInventory(amountToDecrease);

            // Assert
            part.CurrentStock.Should().Be(80);
        }

        // US31: Actualización de inventario
        // Escenario 2: Intentar descontar más del stock disponible (Número inválido/insuficiente)
        [Fact]
        public void DecreaseStock_WithAmountGreaterThanStock_ThrowsInvalidOperationException()
        {
            // Arrange
            var unitPrice = new Money(50.0m, "USD");
            var part = InventoryPart.Create("PN-001", "Filtro", "Filtro de aire", 10, 5, 1, unitPrice);
            var amountToDecrease = 15;

            // Act
            Action act = () => part.DecreaseInventory(amountToDecrease);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
    }
}
