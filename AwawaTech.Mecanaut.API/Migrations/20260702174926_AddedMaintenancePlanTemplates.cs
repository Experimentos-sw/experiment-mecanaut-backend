using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace AwawaTech.Mecanaut.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedMaintenancePlanTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "experiment_surveys",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateTable(
                name: "maintenance_plan_templates",
                columns: table => new
                {
                    maintenance_plan_template_id = table.Column<long>(type: "bigint", nullable: false)
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
                    table.PrimaryKey("p_k_maintenance_plan_templates", x => x.maintenance_plan_template_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "maintenance_plan_template_machines",
                columns: table => new
                {
                    maintenance_plan_template_machine_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    maintenance_plan_template_id = table.Column<long>(type: "bigint", nullable: false),
                    machine_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_maintenance_plan_template_machines", x => x.maintenance_plan_template_machine_id);
                    table.ForeignKey(
                        name: "f_k__maintenance_plan_template_machine__template",
                        column: x => x.maintenance_plan_template_id,
                        principalTable: "maintenance_plan_templates",
                        principalColumn: "maintenance_plan_template_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "maintenance_plan_template_tasks",
                columns: table => new
                {
                    maintenance_plan_template_task_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    maintenance_plan_template_id = table.Column<long>(type: "bigint", nullable: false),
                    task_description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_maintenance_plan_template_tasks", x => x.maintenance_plan_template_task_id);
                    table.ForeignKey(
                        name: "f_k__maintenance_plan_template_task__template",
                        column: x => x.maintenance_plan_template_id,
                        principalTable: "maintenance_plan_templates",
                        principalColumn: "maintenance_plan_template_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_maintenance_plan_template_machines_maintenance_plan_template~",
                table: "maintenance_plan_template_machines",
                column: "maintenance_plan_template_id");

            migrationBuilder.CreateIndex(
                name: "i_x_maintenance_plan_template_tasks_maintenance_plan_template_id",
                table: "maintenance_plan_template_tasks",
                column: "maintenance_plan_template_id");

            migrationBuilder.CreateIndex(
                name: "i_x_maintenance_plan_templates_name_tenant_id",
                table: "maintenance_plan_templates",
                columns: new[] { "name", "tenant_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "maintenance_plan_template_machines");

            migrationBuilder.DropTable(
                name: "maintenance_plan_template_tasks");

            migrationBuilder.DropTable(
                name: "maintenance_plan_templates");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "experiment_surveys",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);
        }
    }
}
