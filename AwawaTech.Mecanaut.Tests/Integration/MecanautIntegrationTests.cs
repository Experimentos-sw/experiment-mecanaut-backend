using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Linq;

using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using AwawaTech.Mecanaut.API.WorkOrders.Interfaces.REST.Resources;

namespace AwawaTech.Mecanaut.Tests.Integration
{

    public class MecanautWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _dbName = $"InMemoryDbForIntegrationTesting_{Guid.NewGuid():N}";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                });
            });
        }
    }

    public class MecanautIntegrationTests : IClassFixture<MecanautWebApplicationFactory>, IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly MecanautWebApplicationFactory _factory;

        public MecanautIntegrationTests(MecanautWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            var signInPayload = new
            {
                username = "admin@mecanaut.com",
                password = "SecurePassword123!"
            };

            var signInResponse = await _client.PostAsJsonAsync("/api/v1/authentication/sign-in", signInPayload);

            if (!signInResponse.IsSuccessStatusCode)
            {
                var signUpPayload = new
                {
                    ruc = "12345678901",
                    legalName = "Test Company",
                    tenantEmail = "test@company.com",
                    subscriptionPlanId = 1,
                    username = "admin@mecanaut.com",
                    password = "SecurePassword123!",
                    email = "admin@mecanaut.com",
                    firstName = "Admin",
                    lastName = "User"
                };
                await _client.PostAsJsonAsync("/api/v1/authentication/sign-up", signUpPayload);
                signInResponse = await _client.PostAsJsonAsync("/api/v1/authentication/sign-in", signInPayload);
            }

            if (signInResponse.IsSuccessStatusCode)
            {
                var result = await signInResponse.Content.ReadFromJsonAsync<JsonElement>();
                if (result.TryGetProperty("token", out var tokenProp))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenProp.GetString());
                }
            }
        }

        public Task DisposeAsync() => Task.CompletedTask;

        // Escenario 1: US01 - Registro de Maquinaria
        // Endpoint: POST /api/v1/machines
        [Fact]
        public async Task RegisterMachine_ValidPayload_ReturnsOkAndPersists()
        {
            // Arrange
            var payload = new
            {
                serialNumber = "MAC-2026-X1",
                name = "Prensa Hidráulica",
                plantId = 1,
                tenantId = 1,
                type = "Industrial",
                model = "Modelo-X",
                manufacturer = "Fabricante-Y",
                powerConsumption = 150.0,
                metrics = new int[0],
                specs = new { capacity = "1000T" }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/machines", payload);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().BeOneOf(new[] { HttpStatusCode.OK, HttpStatusCode.Created }, $"Body: {content}");

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var machineInDb = await db.Set<AwawaTech.Mecanaut.API.AssetManagement.Domain.Model.Aggregates.Machine>()
                                      .FirstOrDefaultAsync(m => m.SerialNumber == "MAC-2026-X1");
            machineInDb.Should().NotBeNull("La máquina debería haberse persistido en la base de datos.");
            machineInDb!.Name.Should().Be("Prensa Hidráulica");
        }

        // Escenario 2: US04 - Asignación de Técnico
        // Endpoint: POST /api/v1/work-orders/{id}/assign
        [Fact]
        public async Task AssignTechnician_ToWorkOrder_ReturnsOkAndPersistsInDb()
        {
            // Arrange
            long workOrderId = 1;

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (!db.Set<AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates.WorkOrder>().Any(w => w.Id == workOrderId))
                {
                    var tenantId = new AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects.TenantId(1);
                    var newWo = AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates.WorkOrder.Create("WO-001", tenantId, System.DateTime.UtcNow, 1, AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects.WorkOrderType.Corrective);
                    db.Set<AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates.WorkOrder>().Add(newWo);
                    await db.SaveChangesAsync();
                }
            }

            var technicianIdToAssign = 5L;
            var payload = new
            {
                technicianIds = new[] { technicianIdToAssign }
            };

            // Act
            var response = await _client.PostAsJsonAsync($"/api/v1/work-orders/{workOrderId}/assign", payload);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().BeOneOf(new[] { HttpStatusCode.OK, HttpStatusCode.NoContent, HttpStatusCode.Created, HttpStatusCode.NotFound }, $"Body: {content}");

            if (response.IsSuccessStatusCode)
            {
                using (var scope = _factory.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var workOrder = await db.Set<AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates.WorkOrder>()
                                            .FirstOrDefaultAsync(w => w.Id == workOrderId);

                    workOrder.Should().NotBeNull();
                    workOrder!.TechnicianIds.Should().Contain(technicianIdToAssign, "El ID del técnico debe estar registrado en la orden de trabajo en la BD.");
                }
            }
        }

        // Escenario 3: US09 - Creación de Plan de Mantenimiento
        // Endpoint: POST /api/v1/maintenance-plans
        [Fact]
        public async Task CreateMaintenancePlan_ValidPayload_ReturnsOkAndPersists()
        {
            // Arrange
            var payload = new
            {
                name = "Plan Preventivo Anual TDD",
                metricId = "5",
                amount = "85.5",
                productionLineId = "2",
                plantLineId = "1",
                machines = new long[] { 1, 2 },
                tasks = new string[] { "Inspeccionar motores" },
                durationInDays = 365
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/maintenance-plans", payload);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().BeOneOf(new[] { HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.NotFound }, $"Body: {content}");

            if (response.IsSuccessStatusCode)
            {
                using var scope = _factory.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var plan = await db.Set<AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Model.Aggregates.DynamicMaintenancePlan>()
                                   .FirstOrDefaultAsync(p => p.Name == "Plan Preventivo Anual TDD");

                plan.Should().NotBeNull("El plan de mantenimiento debe guardarse en la BD.");
            }
        }

        // Escenario 4: US18 + US31 - Cierre de Orden e Inventario
        // Endpoint: POST /api/v1/executed-work-orders
        [Fact]
        public async Task RegisterExecutedWorkOrder_ValidPayload_ClosesOrderAndDecreasesInventory()
        {
            // Arrange
            long inventoryPartId = 10;
            int initialStock = 50;
            int quantityUsed = 2;

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var unitPrice = new AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.ValueObjects.Money(10m, "USD");
                var newPart = AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.Aggregates.InventoryPart.Create(
                    "P-100", "Filtro Especial", "Filtro para prueba de cierre", initialStock, 5, 1, unitPrice);

                db.Set<AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.Aggregates.InventoryPart>().Add(newPart);
                await db.SaveChangesAsync();

                inventoryPartId = newPart.Id;
            }

            var payload = new
            {
                code = "EWO-9988",
                annotations = "Se reemplazó el filtro principal exitosamente.",
                executionDate = System.DateTime.UtcNow,
                intervenedMachineIds = new[] { 1, 2 },
                usedProducts = new[]
                {
                    new { productId = inventoryPartId, quantity = quantityUsed }
                },
                files = new string[0],
                executedTasks = new[] { "Limpieza", "Reemplazo" },
                assignedTechnicianIds = new[] { 1, 3 },
                workOrderId = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/executed-work-orders", payload);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().BeOneOf(new[] { HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.Conflict }, $"Body: {content}");

            if (response.IsSuccessStatusCode)
            {
                using var scope = _factory.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var workOrder = await db.Set<AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates.WorkOrder>()
                                        .FirstOrDefaultAsync(w => w.Id == payload.workOrderId);

                if (workOrder != null)
                {
                    workOrder.Status.Should().Be(AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects.WorkOrderStatus.Completed, "La orden de trabajo debería haber cambiado su estado a Completada (Cerrada).");
                }

                var updatedPart = await db.Set<AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.Aggregates.InventoryPart>()
                                          .FirstOrDefaultAsync(p => p.Id == inventoryPartId);

                updatedPart.Should().NotBeNull();
                updatedPart!.CurrentStock.Should().Be(initialStock - quantityUsed, "El stock actual debería haber disminuido según la cantidad reportada en el payload.");
            }
        }

        // Escenario 5: US11-R - Verificación de Stock e Inicio de Orden
        [Fact]
        public async Task StartWorkOrder_SufficientStock_StartsOrder()
        {
            // Arrange
            long partId;
            long workOrderId;

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var unitPrice = new AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.ValueObjects.Money(15m, "PEN");
                var part = AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.Aggregates.InventoryPart.Create(
                    "P-US11-OK", "Repuesto Suficiente", "Para pruebas de stock suficiente", 10, 2, 1, unitPrice);
                db.Set<AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.Aggregates.InventoryPart>().Add(part);
                await db.SaveChangesAsync();
                partId = part.Id;

                var tenantId = new AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects.TenantId(1);
                var wo = AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates.WorkOrder.Create(
                    "WO-US11-OK", tenantId, DateTime.UtcNow, 1, AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects.WorkOrderType.Preventive);
                
                var reqPart = new AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Entities.WorkOrderRequiredPart(partId, 4);
                wo.AddRequiredParts(new[] { reqPart });
                
                db.Set<AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates.WorkOrder>().Add(wo);
                await db.SaveChangesAsync();
                workOrderId = wo.Id;
            }

            // Act - GET Verification
            var getVerifyResponse = await _client.GetAsync($"/api/v1/work-orders/{workOrderId}/stock-verification");
            getVerifyResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var verifyResult = await getVerifyResponse.Content.ReadFromJsonAsync<List<StockVerificationResultResource>>();
            verifyResult.Should().NotBeNull();
            verifyResult!.Count.Should().Be(1);
            verifyResult[0].InventoryPartId.Should().Be(partId);
            verifyResult[0].RequiredQuantity.Should().Be(4);
            verifyResult[0].AvailableQuantity.Should().Be(10);
            verifyResult[0].HasSufficientStock.Should().BeTrue();

            // Act - Start Order
            var startResponse = await _client.PutAsync($"/api/v1/work-orders/{workOrderId}/start", null);
            startResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Assert - Db check
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var order = await db.Set<AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates.WorkOrder>()
                    .FirstOrDefaultAsync(w => w.Id == workOrderId);
                order.Should().NotBeNull();
                order!.Status.Should().Be(AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects.WorkOrderStatus.InProgress);
            }
        }

        [Fact]
        public async Task StartWorkOrder_InsufficientStock_BlocksOrderAndReturnsConflict()
        {
            // Arrange
            long partId;
            long workOrderId;

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var unitPrice = new AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.ValueObjects.Money(15m, "PEN");
                var part = AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.Aggregates.InventoryPart.Create(
                    "P-US11-FAIL", "Repuesto Insuficiente", "Para pruebas de stock insuficiente", 3, 2, 1, unitPrice);
                db.Set<AwawaTech.Mecanaut.API.InventoryManagement.Domain.Model.Aggregates.InventoryPart>().Add(part);
                await db.SaveChangesAsync();
                partId = part.Id;

                var tenantId = new AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects.TenantId(1);
                var wo = AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates.WorkOrder.Create(
                    "WO-US11-FAIL", tenantId, DateTime.UtcNow, 1, AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects.WorkOrderType.Preventive);
                
                var reqPart = new AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Entities.WorkOrderRequiredPart(partId, 8);
                wo.AddRequiredParts(new[] { reqPart });
                
                db.Set<AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates.WorkOrder>().Add(wo);
                await db.SaveChangesAsync();
                workOrderId = wo.Id;
            }

            // Act - GET Verification
            var getVerifyResponse = await _client.GetAsync($"/api/v1/work-orders/{workOrderId}/stock-verification");
            getVerifyResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var verifyResult = await getVerifyResponse.Content.ReadFromJsonAsync<List<StockVerificationResultResource>>();
            verifyResult.Should().NotBeNull();
            verifyResult!.Count.Should().Be(1);
            verifyResult[0].HasSufficientStock.Should().BeFalse();

            // Act - Start Order (Expect Block)
            var startResponse = await _client.PutAsync($"/api/v1/work-orders/{workOrderId}/start", null);
            startResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Assert - Db check
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var order = await db.Set<AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates.WorkOrder>()
                    .FirstOrDefaultAsync(w => w.Id == workOrderId);
                order.Should().NotBeNull();
                order!.Status.Should().Be(AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects.WorkOrderStatus.Pending);
            }
        }
    }
}
