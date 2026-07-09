using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwawaTech.Mecanaut.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExperimentSurvey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "duration_seconds",
                table: "experiment_surveys",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_step",
                table: "experiment_surveys",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duration_seconds",
                table: "experiment_surveys");

            migrationBuilder.DropColumn(
                name: "last_step",
                table: "experiment_surveys");
        }
    }
}
