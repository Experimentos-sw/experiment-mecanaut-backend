using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace AwawaTech.Mecanaut.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMaintenancePlanIdToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dynamic_maintenance_plans",
                columns: table => new
                {
                    dynamic_maintenance_plan_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    metric_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<double>(type: "double", nullable: false),
                    production_line_id = table.Column<long>(type: "bigint", nullable: false),
                    plant_line_id = table.Column<long>(type: "bigint", nullable: false),
                    tenant_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_dynamic_maintenance_plans", x => x.dynamic_maintenance_plan_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "executed_work_orders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "longtext", nullable: false),
                    annotations = table.Column<string>(type: "longtext", nullable: false),
                    tenant_id = table.Column<long>(type: "bigint", nullable: false),
                    execution_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    production_line_id = table.Column<long>(type: "bigint", nullable: false),
                    intervened_machine_ids = table.Column<string>(type: "longtext", nullable: false),
                    assigned_technician_ids = table.Column<string>(type: "longtext", nullable: false),
                    executed_tasks = table.Column<string>(type: "longtext", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_executed_work_orders", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "experiment_surveys",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    maintenance_plan_id = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    variant = table.Column<string>(type: "longtext", nullable: false),
                    comment = table.Column<string>(type: "longtext", nullable: true),
                    submitted_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_experiment_surveys", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "inventory_parts",
                columns: table => new
                {
                    inventory_part_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    part_number = table.Column<string>(type: "varchar(255)", nullable: false),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: false),
                    current_stock = table.Column<int>(type: "int", nullable: false),
                    min_stock = table.Column<int>(type: "int", nullable: false),
                    plant_id = table.Column<long>(type: "bigint", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_inventory_parts", x => x.inventory_part_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "machine_metrics",
                columns: table => new
                {
                    machine_metrics_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    machine_id = table.Column<long>(type: "bigint", nullable: false),
                    tenant_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_machine_metrics", x => x.machine_metrics_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "machines",
                columns: table => new
                {
                    machine_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    serial_number = table.Column<string>(type: "varchar(255)", nullable: false),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    manufacturer = table.Column<string>(type: "longtext", nullable: false),
                    model = table.Column<string>(type: "longtext", nullable: false),
                    type = table.Column<string>(type: "longtext", nullable: false),
                    power_consumption = table.Column<double>(type: "double", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    last_maintenance = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    next_maintenance = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    plant_id = table.Column<long>(type: "bigint", nullable: false),
                    production_line_id = table.Column<long>(type: "bigint", nullable: true),
                    tenant_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_machines", x => x.machine_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metric_definitions",
                columns: table => new
                {
                    metric_definition_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_metric_definitions", x => x.metric_definition_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "plants",
                columns: table => new
                {
                    plant_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    address = table.Column<string>(type: "longtext", nullable: false),
                    city = table.Column<string>(type: "longtext", nullable: false),
                    country = table.Column<string>(type: "longtext", nullable: false),
                    phone = table.Column<string>(type: "longtext", nullable: false),
                    email = table.Column<string>(type: "longtext", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    tenant_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_plants", x => x.plant_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "production_lines",
                columns: table => new
                {
                    production_line_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    code = table.Column<string>(type: "longtext", nullable: false),
                    units_per_hour = table.Column<double>(type: "double", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    plant_id = table.Column<long>(type: "bigint", nullable: false),
                    tenant_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_production_lines", x => x.production_line_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "purchase_orders",
                columns: table => new
                {
                    purchase_order_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    order_number = table.Column<string>(type: "longtext", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    order_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    delivery_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    inventory_part_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    total_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    plant_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_purchase_orders", x => x.purchase_order_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_roles", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "skills",
                columns: table => new
                {
                    skill_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    category = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    tenant_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_skills", x => x.skill_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "subscription_plans",
                columns: table => new
                {
                    subscription_plan_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    currency = table.Column<string>(type: "longtext", nullable: false),
                    max_machines = table.Column<int>(type: "int", nullable: false),
                    max_users = table.Column<int>(type: "int", nullable: false),
                    support_priority = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    predictive_maintenance = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    advanced_analytics = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_subscription_plans", x => x.subscription_plan_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tenants",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ruc = table.Column<string>(type: "longtext", nullable: false),
                    legal_name = table.Column<string>(type: "longtext", nullable: false),
                    code = table.Column<string>(type: "longtext", nullable: false),
                    commercial_name = table.Column<string>(type: "longtext", nullable: true),
                    address = table.Column<string>(type: "longtext", nullable: true),
                    city = table.Column<string>(type: "longtext", nullable: true),
                    country = table.Column<string>(type: "longtext", nullable: true),
                    phone_number = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    website = table.Column<string>(type: "longtext", nullable: true),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    subscription_plan_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_tenants", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "longtext", nullable: false),
                    password_hash = table.Column<string>(type: "longtext", nullable: false),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    first_name = table.Column<string>(type: "longtext", nullable: true),
                    last_name = table.Column<string>(type: "longtext", nullable: true),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    tenant_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_users", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "work_orders",
                columns: table => new
                {
                    work_order_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "longtext", nullable: false),
                    tenant_id = table.Column<long>(type: "bigint", nullable: false),
                    machine_ids = table.Column<string>(type: "longtext", nullable: false),
                    technician_ids = table.Column<string>(type: "longtext", nullable: false),
                    tasks = table.Column<string>(type: "longtext", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    production_line_id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<string>(type: "longtext", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_work_orders", x => x.work_order_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dynamic_maintenance_plan_machines",
                columns: table => new
                {
                    d_plan_machine_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    plan_id = table.Column<long>(type: "bigint", nullable: false),
                    machine_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_dynamic_maintenance_plan_machines", x => x.d_plan_machine_id);
                    table.ForeignKey(
                        name: "f_k__dynamic_maintenance_plan_machine__plan",
                        column: x => x.plan_id,
                        principalTable: "dynamic_maintenance_plans",
                        principalColumn: "dynamic_maintenance_plan_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dynamic_maintenance_plan_tasks",
                columns: table => new
                {
                    d_plan_task_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    plan_id = table.Column<long>(type: "bigint", nullable: false),
                    task_description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_dynamic_maintenance_plan_tasks", x => x.d_plan_task_id);
                    table.ForeignKey(
                        name: "f_k__dynamic_maintenance_plan_task__plan",
                        column: x => x.plan_id,
                        principalTable: "dynamic_maintenance_plans",
                        principalColumn: "dynamic_maintenance_plan_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "execution_images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    executed_work_order_id = table.Column<long>(type: "bigint", nullable: false),
                    image_url = table.Column<string>(type: "longtext", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_execution_images", x => x.id);
                    table.ForeignKey(
                        name: "f_k_execution_images_executed_work_orders_executed_work_order_id",
                        column: x => x.executed_work_order_id,
                        principalTable: "executed_work_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "used_products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    executed_work_order_id = table.Column<long>(type: "bigint", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_used_products", x => x.id);
                    table.ForeignKey(
                        name: "f_k_used_products_executed_work_orders_executed_work_order_id",
                        column: x => x.executed_work_order_id,
                        principalTable: "executed_work_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metric_readings",
                columns: table => new
                {
                    metric_reading_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    machine_id = table.Column<long>(type: "bigint", nullable: false),
                    metric_id = table.Column<long>(type: "bigint", nullable: false),
                    value = table.Column<double>(type: "double", nullable: false),
                    measured_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    tenant_id = table.Column<long>(type: "bigint", nullable: false),
                    machine_metrics_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_metric_readings", x => x.metric_reading_id);
                    table.ForeignKey(
                        name: "f_k_metric_readings_machine_metrics_machine_metrics_id",
                        column: x => x.machine_metrics_id,
                        principalTable: "machine_metrics",
                        principalColumn: "machine_metrics_id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    roles_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_user_roles", x => new { x.roles_id, x.user_id });
                    table.ForeignKey(
                        name: "f_k_user_roles_roles_roles_id",
                        column: x => x.roles_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_dynamic_maintenance_plan_machines_plan_id",
                table: "dynamic_maintenance_plan_machines",
                column: "plan_id");

            migrationBuilder.CreateIndex(
                name: "i_x_dynamic_maintenance_plan_tasks_plan_id",
                table: "dynamic_maintenance_plan_tasks",
                column: "plan_id");

            migrationBuilder.CreateIndex(
                name: "i_x_dynamic_maintenance_plans_name_tenant_id",
                table: "dynamic_maintenance_plans",
                columns: new[] { "name", "tenant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_execution_images_executed_work_order_id",
                table: "execution_images",
                column: "executed_work_order_id");

            migrationBuilder.CreateIndex(
                name: "i_x_inventory_parts_part_number",
                table: "inventory_parts",
                column: "part_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_machine_metrics_machine_id",
                table: "machine_metrics",
                column: "machine_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_machines_serial_number",
                table: "machines",
                column: "serial_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_metric_definitions_name",
                table: "metric_definitions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_metric_readings_machine_metrics_id",
                table: "metric_readings",
                column: "machine_metrics_id");

            migrationBuilder.CreateIndex(
                name: "idx_machine_metric_time",
                table: "metric_readings",
                columns: new[] { "machine_id", "metric_id", "measured_at" });

            migrationBuilder.CreateIndex(
                name: "i_x_skills_name_tenant_id",
                table: "skills",
                columns: new[] { "name", "tenant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_used_products_executed_work_order_id",
                table: "used_products",
                column: "executed_work_order_id");

            migrationBuilder.CreateIndex(
                name: "i_x_user_roles_user_id",
                table: "user_roles",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dynamic_maintenance_plan_machines");

            migrationBuilder.DropTable(
                name: "dynamic_maintenance_plan_tasks");

            migrationBuilder.DropTable(
                name: "execution_images");

            migrationBuilder.DropTable(
                name: "experiment_surveys");

            migrationBuilder.DropTable(
                name: "inventory_parts");

            migrationBuilder.DropTable(
                name: "machines");

            migrationBuilder.DropTable(
                name: "metric_definitions");

            migrationBuilder.DropTable(
                name: "metric_readings");

            migrationBuilder.DropTable(
                name: "plants");

            migrationBuilder.DropTable(
                name: "production_lines");

            migrationBuilder.DropTable(
                name: "purchase_orders");

            migrationBuilder.DropTable(
                name: "skills");

            migrationBuilder.DropTable(
                name: "subscription_plans");

            migrationBuilder.DropTable(
                name: "tenants");

            migrationBuilder.DropTable(
                name: "used_products");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "work_orders");

            migrationBuilder.DropTable(
                name: "dynamic_maintenance_plans");

            migrationBuilder.DropTable(
                name: "machine_metrics");

            migrationBuilder.DropTable(
                name: "executed_work_orders");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
