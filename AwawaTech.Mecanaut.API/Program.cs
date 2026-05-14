using AwawaTech.Mecanaut.API.IAM.Application.Internal.CommandServices;
using AwawaTech.Mecanaut.API.IAM.Application.Internal.OutboundServices;
using AwawaTech.Mecanaut.API.IAM.Application.Internal.QueryServices;
using AwawaTech.Mecanaut.API.IAM.Domain.Repositories;
using AwawaTech.Mecanaut.API.IAM.Domain.Services;
using AwawaTech.Mecanaut.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using AwawaTech.Mecanaut.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using AwawaTech.Mecanaut.API.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using AwawaTech.Mecanaut.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using AwawaTech.Mecanaut.API.IAM.Infrastructure.Tokens.JWT.Services;
using AwawaTech.Mecanaut.API.IAM.Interfaces.ACL;
using AwawaTech.Mecanaut.API.IAM.Interfaces.ACL.Services;
using AwawaTech.Mecanaut.API.Shared.Domain.Repositories;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Multitenancy;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Interfaces.ASP.Middleware;
using Microsoft.Extensions.Hosting;
using AwawaTech.Mecanaut.API.IAM.Application.Internal.EventHandlers;
using AwawaTech.Mecanaut.API.Shared.Domain.Events;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.DomainEvents;
using AwawaTech.Mecanaut.API.AssetManagement.Domain.Repositories;
using AwawaTech.Mecanaut.API.AssetManagement.Domain.Services;
using AwawaTech.Mecanaut.API.AssetManagement.Application.Internal.CommandServices;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Interfaces.ASP.Filters;
using AwawaTech.Mecanaut.API.AssetManagement.Application.Internal.QueryServices;
using AwawaTech.Mecanaut.API.InventoryManagement.Domain.Repositories;
using AwawaTech.Mecanaut.API.InventoryManagement.Domain.Services;
using AwawaTech.Mecanaut.API.InventoryManagement.Application.Internal.CommandServices;
using AwawaTech.Mecanaut.API.InventoryManagement.Application.Internal.QueryServices;
using AwawaTech.Mecanaut.API.InventoryManagement.Infrastructure.Persistence.EFC.Repositories;
using AwawaTech.Mecanaut.API.InventoryManagement.Interfaces.REST.Transform;
using AwawaTech.Mecanaut.API.Competency.Domain.Repositories;
using AwawaTech.Mecanaut.API.Competency.Domain.Services;
using AwawaTech.Mecanaut.API.Competency.Application.Internal.CommandServices;
using AwawaTech.Mecanaut.API.Competency.Application.Internal.QueryServices;
using AwawaTech.Mecanaut.API.Competency.Infrastructure.Persistence.EFC.Repositories;
using AwawaTech.Mecanaut.API.ConditionMonitoring.Domain.Repositories;
using AwawaTech.Mecanaut.API.ConditionMonitoring.Domain.Services;
using AwawaTech.Mecanaut.API.ConditionMonitoring.Application.Internal.CommandServices;
using AwawaTech.Mecanaut.API.ConditionMonitoring.Application.Internal.QueryServices;
using AwawaTech.Mecanaut.API.ConditionMonitoring.Infrastructure.Persistence.EFC.Repositories;
using AwawaTech.Mecanaut.API.ConditionMonitoring.Infrastructure.OutboundServices;
using AwawaTech.Mecanaut.API.ConditionMonitoring.Application.Internal.EventHandlers;
using AwawaTech.Mecanaut.API.ConditionMonitoring.Application.Internal.OutboundServices;
using AwawaTech.Mecanaut.API.Subscription.Domain.Repositories;
using AwawaTech.Mecanaut.API.Subscription.Domain.Services;
using AwawaTech.Mecanaut.API.Subscription.Application.Internal.CommandServices;
using AwawaTech.Mecanaut.API.Subscription.Application.Internal.QueryServices;
using AwawaTech.Mecanaut.API.Subscription.Infrastructure.Persistence.EFC.Repositories;
using AwawaTech.Mecanaut.API.Subscription.Application.Internal.EventHandlers;

using AwawaTech.Mecanaut.API.AssetManagement.Infrastructure.Persistence.EFC.Repositories;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Repositories;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Domain.Services;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Infrastructure.Persistence.EFC.Repositories;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Application.Internal.CommandServices;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Application.Internal.QueryServices;
using AwawaTech.Mecanaut.API.DynamicMaintenancePlanning.Interfaces.REST.Transform;

using AwawaTech.Mecanaut.API.WorkOrders.Domain.Repositories;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Services;
using AwawaTech.Mecanaut.API.WorkOrders.Infrastructure.Persistence.EFC.Repositories;
using AwawaTech.Mecanaut.API.WorkOrders.Application.Internal.CommandServices;
using AwawaTech.Mecanaut.API.WorkOrders.Application.Internal.QueryServices;

using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Domain.Repositories;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Domain.Services;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Infrastructure.Persistence.EFC.Repositories;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Application.Internal.CommandServices;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Application.Internal.QueryServices;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Interfaces.REST.Transform;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Application.Internal.OutboundServices;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Infrastructure.OutboundServices.Services;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Storage;
using AwawaTech.Mecanaut.API.Shared.Domain.Services;
using AwawaTech.Mecanaut.API.WorkOrders.Application.Internal.CommandServices;


var builder = WebApplication.CreateBuilder(args);

// ───────────── Controllers & routing ─────────────
builder.Services.AddRouting(o => o.LowercaseUrls = true);
builder.Services.AddControllers(o =>
{
    o.Conventions.Add(new KebabCaseRouteNamingConvention());
    o.Filters.Add<ApiExceptionFilter>(); // Filtro global
});

// ───────────── Autenticación "passthrough" ─────────────
builder.Services.AddAuthentication("Custom")
       .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions,
                 AwawaTech.Mecanaut.API.IAM.Infrastructure.Authorization.PassthroughAuthenticationHandler>
                 ("Custom", _ => { });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", p => p.RequireRole("RoleAdmin"));
});

// ───────────── CORS ─────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        p => p.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ───────────── DbContext (MySQL / InMemory para Tests) ─────────────

// 1. Verificamos que NO estemos en el entorno de pruebas
if (builder.Environment.EnvironmentName != "Testing")
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                          ?? throw new InvalidOperationException("Connection string not found.");

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        if (builder.Environment.IsDevelopment())
            options.UseMySQL(connectionString)
                   .LogTo(Console.WriteLine, LogLevel.Information)
                   .EnableSensitiveDataLogging()
                   .EnableDetailedErrors();
        else
            options.UseMySQL(connectionString)
                   .LogTo(Console.WriteLine, LogLevel.Error);
    });
}

// ───────────── Swagger / OpenAPI ─────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AwawaTech.Mecanaut.API",
        Version = "v1",
        Description = "AwawaTech Mecanaut API",
        TermsOfService = new Uri("https://awawatech.com/tos"),
        Contact = new OpenApiContact
        {
            Name = "AwawaTech",
            Email = "contact@awawatech.com"
        },
        License = new OpenApiLicense
        {
            Name = "Apache 2.0",
            Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
        }
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
            },
            Array.Empty<string>()
        }
    });
    options.EnableAnnotations();
});

// Dependency Injection

// Shared Bounded Context
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ApiExceptionFilter>();
builder.Services.AddSingleton<TenantContextHelper>();

// Publishing Bounded Context
// AssetManagement Bounded Context DI
builder.Services.AddScoped<IPlantRepository, PlantRepository>();
builder.Services.AddScoped<IProductionLineRepository, ProductionLineRepository>();
builder.Services.AddScoped<IMachineRepository, MachineRepository>();

builder.Services.AddScoped<IPlantCommandService, PlantCommandService>();
builder.Services.AddScoped<IPlantQueryService, PlantQueryService>();

builder.Services.AddScoped<IProductionLineCommandService, ProductionLineCommandService>();
builder.Services.AddScoped<IProductionLineQueryService, ProductionLineQueryService>();

builder.Services.AddScoped<IMachineCommandService, MachineCommandService>();
builder.Services.AddScoped<IMachineQueryService, MachineQueryService>();

// Competency Bounded Context
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<ISkillCommandService, SkillCommandService>();
builder.Services.AddScoped<ISkillQueryService, SkillQueryService>();

// Profiles Bounded Context Dependency Injection Configuration

// IAM Bounded Context Injection Configuration

// TokenSettings Configuration

builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IRoleCommandService, RoleCommandService>();
builder.Services.AddScoped<IRoleQueryService, RoleQueryService>();
builder.Services.AddScoped<ITenantCommandService, TenantCommandService>();
builder.Services.AddScoped<ITenantQueryService, TenantQueryService>();
builder.Services.AddHostedService<SeedRolesHostedService>();


// Cloudinary
builder.Services.AddScoped<IImageStorageService, CloudinaryService>();


// ConditionMonitoring Bounded Context
builder.Services.AddScoped<IMachineMetricsRepository, MachineMetricsRepository>();
builder.Services.AddScoped<IMetricDefinitionRepository, MetricDefinitionRepository>();
builder.Services.AddScoped<IMetricReadingRepository, MetricReadingRepository>();

builder.Services.AddScoped<IMachineMetricsCommandService, MachineMetricsCommandService>();
builder.Services.AddScoped<IMetricDefinitionCommandService, MetricDefinitionCommandService>();
builder.Services.AddScoped<IMachineMetricsQueryService, MachineMetricsQueryService>();
builder.Services.AddScoped<IMetricQueryService, MetricQueryService>();

builder.Services.AddScoped<IMachineCatalogAcl, MachineCatalogAcl>();

builder.Services.AddHostedService<MetricsSeedHostedService>();

builder.Services.AddScoped<IDynamicMaintenancePlanningAcl, DynamicMaintenancePlanningAcl>();
builder.Services.AddScoped<IWorkOrderAcl, WorkOrderAcl>();



// Repositorios
builder.Services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();

// Servicios de comando y consulta
builder.Services.AddScoped<ISubscriptionPlanCommandService, SubscriptionPlanCommandService>();
builder.Services.AddScoped<ISubscriptionPlanQueryService, SubscriptionPlanQueryService>();

builder.Services.AddHostedService<SubscriptionPlansSeedHostedService>();

// Registrar ACL de IAM
builder.Services.AddScoped<ISubscriptionPlanAcl, SubscriptionPlanAcl>();

// InventoryManagement Bounded Context
builder.Services.AddScoped<IInventoryPartRepository, InventoryPartRepository>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();

builder.Services.AddScoped<IInventoryPartCommandService, InventoryPartCommandService>();
builder.Services.AddScoped<IInventoryPartQueryService, InventoryPartQueryService>();

builder.Services.AddScoped<IPurchaseOrderCommandService, PurchaseOrderCommandService>();
builder.Services.AddScoped<IPurchaseOrderQueryService, PurchaseOrderQueryService>();

builder.Services.AddScoped<IInventoryPartResourceAssembler, InventoryPartResourceAssembler>();
builder.Services.AddScoped<IPurchaseOrderResourceAssembler, PurchaseOrderResourceAssembler>();
builder.Services.AddScoped<UpdateInventoryPartCommandFromResourceAssembler>();



// DynamicMaintenancePlanning Bounded Context
builder.Services.AddScoped<IDynamicMaintenancePlanRepository, DynamicMaintenancePlanRepository>();
builder.Services.AddScoped<IDynamicMaintenancePlanCommandService, DynamicMaintenancePlanCommandService>();
builder.Services.AddScoped<IDynamicMaintenancePlanQueryService, DynamicMaintenancePlanQueryService>();

//(9)
builder.Services.AddScoped<DynamicMaintenancePlanToResourceAssembler>();
builder.Services.AddScoped<SaveDynamicMaintenancePlanCommandFromResourceAssembler>();
builder.Services.AddScoped<DynamicMaintenancePlanWithDetailsToResourceAssembler>();

//Work Oders

// Work Orders Bounded Context
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<IWorkOrderCommandService, WorkOrderCommandService>();
builder.Services.AddScoped<IWorkOrderQueryService, WorkOrderQueryService>();


// Execution Bounded Context
builder.Services.AddScoped<IExecutedWorkOrderRepository, ExecutedWorkOrderRepository>();
builder.Services.AddScoped<IExecutedWorkOrderCommandService, ExecutedWorkOrderCommandService>();
builder.Services.AddScoped<IExecutedWorkOrderQueryService, ExecutedWorkOrderQueryService>();
builder.Services.AddScoped<SaveExecutedWorkOrderCommandFromResourceAssembler>();
builder.Services.AddScoped<IInventoryManagementAcl, InventoryManagementAcl>();
builder.Services.AddScoped<IWorkOrderExecAcl, WorkOrderExecAcl>();

// ───────────── Build & DB ensure ─────────────a
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// ───────────── HTTP pipeline ─────────────
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAllPolicy");

// Habilita el enrutamiento para que el endpoint esté disponible en middlewares anteriores
app.UseRouting();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
// Aplicar autorización solo a rutas que no sean de Swagger
app.UseWhen(
    ctx => !ctx.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase),
    subApp => subApp.UseRequestAuthorization());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
