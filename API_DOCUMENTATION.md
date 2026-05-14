# Documentación de API RESTful - AwawaTech Mecanaut

## Bounded Context: Authentication

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/v1/authentication/sign-in` | `POST` | **Sign in**<br><br>_Sign in a user_ | - | **Esquema:** `SignInResource`<br><br>**Propiedades:**<br>- `username`: string<br>- `password`: string | **200**: The user was authenticated <br>Esq: `AuthenticatedUserResource` |
| `/api/v1/authentication/sign-up` | `POST` | **Sign-up**<br><br>_Sign up a new user_ | - | **Esquema:** `SignUpResource`<br><br>**Propiedades:**<br>- `ruc`: string<br>- `legalName`: string<br>- `commercialName`: string<br>- `address`: string<br>- `city`: string<br>- `country`: string<br>- `tenantPhone`: string<br>- `tenantEmail`: string<br>- `website`: string<br>- `subscriptionPlanId`: integer<br>- `username`: string<br>- `password`: string<br>- `email`: string<br>- `firstName`: string<br>- `lastName`: string | **201**: The user was created successfully  |

<br>

## Bounded Context: DynamicMaintenancePlans

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/v1/dynamic-maintenance-plans` | `GET` | **/api/v1/dynamic-maintenance-plans** | `plantLineId`<br>(query, Opt)<br>_Type:_ string | - | **200**: OK <br>Esq: `Array of DynamicMaintenancePlanWithDetailsResource` |
| `/api/v1/dynamic-maintenance-plans` | `POST` | **/api/v1/dynamic-maintenance-plans** | - | **Esquema:** `SaveDynamicMaintenancePlanResource`<br><br>**Propiedades:**<br>- `name`: string<br>- `metricId`: string<br>- `amount`: string<br>- `productionLineId`: string<br>- `plantLineId`: string<br>- `machines`: array<br>- `tasks`: array | **200**: OK <br>Esq: `DynamicMaintenancePlanResource` |
| `/api/v1/dynamic-maintenance-plans/{id}` | `GET` | **/api/v1/dynamic-maintenance-plans/{id}** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `DynamicMaintenancePlanResource` |
| `/api/v1/dynamic-maintenance-plans/test-plan-id` | `GET` | **/api/v1/dynamic-maintenance-plans/test-plan-id** | `machineId`<br>(query, Opt)<br>_Type:_ integer (int64)<br><br>`metricId`<br>(query, Opt)<br>_Type:_ integer (int64)<br><br>`amount`<br>(query, Opt)<br>_Type:_ number (double) | - | **200**: OK <br>Esq: `integer` |

<br>

## Bounded Context: ExecutedWorkOrders

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/v1/executed-work-orders` | `POST` | **/api/v1/executed-work-orders** | - | **Esquema:** `SaveExecutedWorkOrderResource`<br><br>**Propiedades:**<br>- `code`: string<br>- `annotations`: string<br>- `executionDate`: string<br>- `productionLineId`: integer<br>- `intervenedMachineIds`: array<br>- `assignedTechnicianIds`: array<br>- `executedTasks`: array<br>- `usedProducts`: array<br>- `files`: array<br>- `workOrderId`: integer | **200**: OK  |
| `/api/v1/executed-work-orders/{id}` | `GET` | **/api/v1/executed-work-orders/{id}** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK  |
| `/api/v1/executed-work-orders/production-line/{lineId}` | `GET` | **/api/v1/executed-work-orders/production-line/{lineId}** | `lineId`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK  |

<br>

## Bounded Context: ImageStorage

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/image-storage/upload` | `POST` | **/api/image-storage/upload** | - | **Tipo:** `object` | **200**: OK  |

<br>

## Bounded Context: InventoryParts

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/inventory-parts` | `GET` | **/api/inventory-parts** | `plantId`<br>(query, Opt)<br>_Type:_ integer (int32) | - | **200**: OK <br>Esq: `Array of InventoryPartResource` |
| `/api/inventory-parts` | `POST` | **/api/inventory-parts** | - | **Esquema:** `CreateInventoryPartResource`<br><br>**Propiedades:**<br>- `code`: string<br>- `name`: string<br>- `description`: string<br>- `currentStock`: integer<br>- `minStock`: integer<br>- `unitPrice`: number<br>- `plantId`: integer | **201**: Created <br>Esq: `InventoryPartResource`<br><br>**400**: Bad Request <br>Esq: `ProblemDetails` |
| `/api/inventory-parts/{id}` | `GET` | **/api/inventory-parts/{id}** | `id`<br>(path, Req)<br>_Type:_ string | - | **200**: OK <br>Esq: `InventoryPartResource`<br><br>**404**: Not Found <br>Esq: `ProblemDetails` |
| `/api/inventory-parts/{id}` | `PUT` | **/api/inventory-parts/{id}** | `id`<br>(path, Req)<br>_Type:_ string | **Esquema:** `UpdateInventoryPartResource`<br><br>**Propiedades:**<br>- `description`: string<br>- `currentStock`: integer<br>- `minStock`: integer<br>- `unitPrice`: number | **200**: OK <br>Esq: `InventoryPartResource`<br><br>**400**: Bad Request <br>Esq: `ProblemDetails`<br><br>**404**: Not Found <br>Esq: `ProblemDetails` |
| `/api/inventory-parts/{id}` | `DELETE` | **/api/inventory-parts/{id}** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **204**: No Content <br><br>**404**: Not Found <br>Esq: `ProblemDetails` |
| `/api/inventory-parts/{id}/decrease` | `PUT` | **/api/inventory-parts/{id}/decrease** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | **Tipo:** `integer` | **200**: OK  |

<br>

## Bounded Context: MachineMetrics

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/v1/machines/{machineId}/metrics` | `POST` | **/api/v1/machines/{machineId}/metrics** | `machineId`<br>(path, Req)<br>_Type:_ integer (int64) | **Esquema:** `RecordMetricResource`<br><br>**Propiedades:**<br>- `metricId`: integer<br>- `value`: number<br>- `measuredAt`: string | **200**: OK  |
| `/api/v1/machines/{machineId}/metrics` | `GET` | **/api/v1/machines/{machineId}/metrics** | `machineId`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `Array of CurrentMetricResource` |
| `/api/v1/machines/{machineId}/metrics/{metricId}` | `GET` | **/api/v1/machines/{machineId}/metrics/{metricId}** | `machineId`<br>(path, Req)<br>_Type:_ integer (int64)<br><br>`metricId`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `CurrentMetricResource` |
| `/api/v1/machines/{machineId}/metrics/{metricId}/readings` | `GET` | **/api/v1/machines/{machineId}/metrics/{metricId}/readings** | `machineId`<br>(path, Req)<br>_Type:_ integer (int64)<br><br>`metricId`<br>(path, Req)<br>_Type:_ integer (int64)<br><br>`from`<br>(query, Opt)<br>_Type:_ string (date-time)<br><br>`to`<br>(query, Opt)<br>_Type:_ string (date-time)<br><br>`page`<br>(query, Opt)<br>_Type:_ integer (int32)<br><br>`size`<br>(query, Opt)<br>_Type:_ integer (int32) | - | **200**: OK <br>Esq: `Array of MetricReadingResource` |

<br>

## Bounded Context: Machines

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/v1/machines` | `GET` | **/api/v1/machines** | - | - | **200**: OK <br>Esq: `Array of MachineResource` |
| `/api/v1/machines` | `POST` | **/api/v1/machines** | - | **Esquema:** `RegisterMachineResource`<br><br>**Propiedades:**<br>- `serialNumber`: string<br>- `name`: string<br>- `manufacturer`: string<br>- `plantId`: integer<br>- `model`: string<br>- `type`: string<br>- `powerConsumption`: number<br>- `metrics`: array | **200**: OK <br>Esq: `MachineResource` |
| `/api/v1/machines/available` | `GET` | **/api/v1/machines/available** | - | - | **200**: OK <br>Esq: `Array of MachineResource` |
| `/api/v1/machines/maintenance-due` | `GET` | **/api/v1/machines/maintenance-due** | - | - | **200**: OK <br>Esq: `Array of MachineResource` |
| `/api/v1/machines/{id}` | `GET` | **/api/v1/machines/{id}** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `MachineResource` |
| `/api/v1/machines/{machineId}/assign` | `PUT` | **/api/v1/machines/{machineId}/assign** | `machineId`<br>(path, Req)<br>_Type:_ integer (int64) | **Esquema:** `AssignMachineResource`<br><br>**Propiedades:**<br>- `productionLineId`: integer | **200**: OK <br>Esq: `MachineResource` |
| `/api/v1/machines/{machineId}/maintenance/start` | `PUT` | **/api/v1/machines/{machineId}/maintenance/start** | `machineId`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK  |
| `/api/v1/machines/{machineId}/maintenance/complete` | `PUT` | **/api/v1/machines/{machineId}/maintenance/complete** | `machineId`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK  |
| `/api/v1/machines/production-line/{lineId}` | `GET` | **/api/v1/machines/production-line/{lineId}** | `lineId`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `Array of MachineResource` |
| `/api/v1/machines/plant/{plantId}` | `GET` | **/api/v1/machines/plant/{plantId}** | `plantId`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `Array of MachineResource` |

<br>

## Bounded Context: MetricDefinitions

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/v1/metric-definitions` | `GET` | **/api/v1/metric-definitions** | - | - | **200**: OK <br>Esq: `Array of MetricDefinitionResource` |

<br>

## Bounded Context: Plants

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/v1/plants` | `GET` | **/api/v1/plants** | - | - | **200**: OK <br>Esq: `Array of PlantResource` |
| `/api/v1/plants` | `POST` | **/api/v1/plants** | - | **Esquema:** `CreatePlantResource`<br><br>**Propiedades:**<br>- `name`: string<br>- `address`: string<br>- `city`: string<br>- `country`: string<br>- `phone`: string<br>- `email`: string | **200**: OK <br>Esq: `PlantResource` |
| `/api/v1/plants/{id}` | `GET` | **/api/v1/plants/{id}** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `PlantResource` |
| `/api/v1/plants/{id}` | `PUT` | **/api/v1/plants/{id}** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | **Esquema:** `UpdatePlantResource`<br><br>**Propiedades:**<br>- `name`: string<br>- `address`: string<br>- `city`: string<br>- `country`: string<br>- `phone`: string<br>- `email`: string | **200**: OK <br>Esq: `PlantResource` |
| `/api/v1/plants/{id}/activate` | `PUT` | **/api/v1/plants/{id}/activate** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK  |
| `/api/v1/plants/{id}/deactivate` | `PUT` | **/api/v1/plants/{id}/deactivate** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK  |

<br>

## Bounded Context: ProductionLines

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/v1/production-lines` | `GET` | **/api/v1/production-lines** | `plantId`<br>(query, Opt)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `Array of ProductionLineResource` |
| `/api/v1/production-lines` | `POST` | **/api/v1/production-lines** | - | **Esquema:** `CreateProductionLineResource`<br><br>**Propiedades:**<br>- `name`: string<br>- `code`: string<br>- `capacityUnitsPerHour`: number<br>- `plantId`: integer | **200**: OK <br>Esq: `ProductionLineResource` |
| `/api/v1/production-lines/running` | `GET` | **/api/v1/production-lines/running** | - | - | **200**: OK <br>Esq: `Array of ProductionLineResource` |
| `/api/v1/production-lines/plant/{plantId}` | `GET` | **/api/v1/production-lines/plant/{plantId}** | `plantId`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `Array of ProductionLineResource` |
| `/api/v1/production-lines/{id}` | `GET` | **/api/v1/production-lines/{id}** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `ProductionLineResource` |
| `/api/v1/production-lines/{id}/start` | `PUT` | **/api/v1/production-lines/{id}/start** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK  |
| `/api/v1/production-lines/{id}/stop` | `PUT` | **/api/v1/production-lines/{id}/stop** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | **Esquema:** `StopProductionResource`<br><br>**Propiedades:**<br>- `reason`: string | **200**: OK  |

<br>

## Bounded Context: PurchaseOrders

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/purchase-orders` | `GET` | **/api/purchase-orders** | `plantId`<br>(query, Opt)<br>_Type:_ integer (int32) | - | **200**: OK <br>Esq: `Array of PurchaseOrderResource` |
| `/api/purchase-orders` | `POST` | **/api/purchase-orders** | - | **Esquema:** `CreatePurchaseOrderResource`<br><br>**Propiedades:**<br>- `orderNumber`: string<br>- `inventoryPartId`: integer<br>- `quantity`: integer<br>- `totalPrice`: number<br>- `plantId`: integer<br>- `deliveryDate`: string | **201**: Created <br>Esq: `PurchaseOrderResource`<br><br>**400**: Bad Request <br>Esq: `ProblemDetails` |
| `/api/purchase-orders/{id}` | `GET` | **/api/purchase-orders/{id}** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `PurchaseOrderResource`<br><br>**404**: Not Found <br>Esq: `ProblemDetails` |
| `/api/purchase-orders/{id}` | `DELETE` | **/api/purchase-orders/{id}** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **204**: No Content <br><br>**404**: Not Found <br>Esq: `ProblemDetails` |
| `/api/purchase-orders/{id}/complete` | `PATCH` | **/api/purchase-orders/{id}/complete** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `PurchaseOrderResource`<br><br>**400**: Bad Request <br>Esq: `ProblemDetails`<br><br>**404**: Not Found <br>Esq: `ProblemDetails` |

<br>

## Bounded Context: Roles

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/v1/roles` | `GET` | **Get all roles** | - | - | **200**: OK  |

<br>

## Bounded Context: Skills

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/v1/skills` | `POST` | **/api/v1/skills** | - | **Esquema:** `CreateSkillResource`<br><br>**Propiedades:**<br>- `name`: string<br>- `description`: string<br>- `category`: string | **200**: OK <br>Esq: `SkillResource` |
| `/api/v1/skills` | `GET` | **/api/v1/skills** | - | - | **200**: OK <br>Esq: `Array of SkillResource` |
| `/api/v1/skills/{skillId}` | `GET` | **/api/v1/skills/{skillId}** | `skillId`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `SkillResource` |
| `/api/v1/skills/{skillId}` | `PUT` | **/api/v1/skills/{skillId}** | `skillId`<br>(path, Req)<br>_Type:_ integer (int64) | **Esquema:** `UpdateSkillResource`<br><br>**Propiedades:**<br>- `name`: string<br>- `description`: string<br>- `category`: string | **200**: OK <br>Esq: `SkillResource` |
| `/api/v1/skills/{skillId}` | `DELETE` | **/api/v1/skills/{skillId}** | `skillId`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK  |

<br>

## Bounded Context: SubscriptionPlans

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/v1/subscription-plans` | `POST` | **/api/v1/subscription-plans** | - | **Esquema:** `CreateSubscriptionPlanResource`<br><br>**Propiedades:**<br>- `name`: string<br>- `description`: string<br>- `price`: number<br>- `currency`: string<br>- `maxMachines`: integer<br>- `maxUsers`: integer<br>- `supportPriority`: boolean<br>- `predictiveMaintenance`: boolean<br>- `advancedAnalytics`: boolean | **200**: OK <br>Esq: `SubscriptionPlanResource` |
| `/api/v1/subscription-plans` | `GET` | **/api/v1/subscription-plans** | - | - | **200**: OK <br>Esq: `Array of SubscriptionPlanResource` |
| `/api/v1/subscription-plans/{id}` | `GET` | **/api/v1/subscription-plans/{id}** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `SubscriptionPlanResource` |
| `/api/v1/subscription-plans/{id}` | `PUT` | **/api/v1/subscription-plans/{id}** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | **Esquema:** `UpdateSubscriptionPlanResource`<br><br>**Propiedades:**<br>- `name`: string<br>- `description`: string<br>- `price`: number<br>- `currency`: string<br>- `maxMachines`: integer<br>- `maxUsers`: integer<br>- `supportPriority`: boolean<br>- `predictiveMaintenance`: boolean<br>- `advancedAnalytics`: boolean | **200**: OK  |
| `/api/v1/subscription-plans/{id}/status` | `PUT` | **/api/v1/subscription-plans/{id}/status** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | **Tipo:** `string` | **200**: OK  |

<br>

## Bounded Context: Users

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/v1/users/{id}` | `GET` | **Get a user by its id**<br><br>_Get a user by its id_ | `id`<br>(path, Req)<br>_Type:_ integer (int32) | - | **200**: The user was found <br>Esq: `UserResource` |
| `/api/v1/users/{id}` | `PUT` | **Update user** | `id`<br>(path, Req)<br>_Type:_ integer (int32) | **Esquema:** `UpdateUserResource`<br><br>**Propiedades:**<br>- `email`: string<br>- `firstName`: string<br>- `lastName`: string<br>- `roles`: array | **200**: OK  |
| `/api/v1/users/{id}` | `DELETE` | **Delete user** | `id`<br>(path, Req)<br>_Type:_ integer (int32) | - | **200**: OK  |
| `/api/v1/users` | `GET` | **Get all users**<br><br>_Get all users_ | - | - | **200**: The users were found <br>Esq: `Array of UserResource` |
| `/api/v1/users` | `POST` | **Create user** | - | **Esquema:** `CreateUserResource`<br><br>**Propiedades:**<br>- `username`: string<br>- `password`: string<br>- `email`: string<br>- `firstName`: string<br>- `lastName`: string<br>- `roles`: array | **200**: OK  |

<br>

## Bounded Context: WorkOrders

| Endpoint | Método | Nombre / Descripción | Parámetros | Body Requerido | Respuestas |
| --- | --- | --- | --- | --- | --- |
| `/api/v1/work-orders` | `POST` | **/api/v1/work-orders** | - | **Esquema:** `CreateWorkOrderResource`<br><br>**Propiedades:**<br>- `code`: string<br>- `date`: string<br>- `productionLineId`: integer<br>- `type`: string<br>- `machineIds`: array<br>- `tasks`: array<br>- `technicianIds`: array | **200**: OK <br>Esq: `WorkOrderResource` |
| `/api/v1/work-orders/{id}` | `GET` | **/api/v1/work-orders/{id}** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `WorkOrderResource` |
| `/api/v1/work-orders/by-production-line/{productionLineId}` | `GET` | **/api/v1/work-orders/by-production-line/{productionLineId}** | `productionLineId`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `Array of WorkOrderResource` |
| `/api/v1/work-orders/by-production-line-to-execute/{productionLineId}` | `GET` | **/api/v1/work-orders/by-production-line-to-execute/{productionLineId}** | `productionLineId`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `Array of WorkOrderResource` |
| `/api/v1/work-orders/{id}/complete` | `PUT` | **/api/v1/work-orders/{id}/complete** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | - | **200**: OK <br>Esq: `WorkOrderResource` |
| `/api/v1/work-orders/{id}/technicians` | `PUT` | **/api/v1/work-orders/{id}/technicians** | `id`<br>(path, Req)<br>_Type:_ integer (int64) | **Tipo:** `array` | **200**: OK <br>Esq: `WorkOrderResource` |

<br>

