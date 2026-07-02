using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace AwawaTech.Mecanaut.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTelemetryFieldsToExperimentSurveys : Migration
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

            migrationBuilder.AddColumn<string>(
                name: "action",
                table: "experiment_surveys",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "experiment_surveys",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "action",
                table: "experiment_surveys");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "experiment_surveys");

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
