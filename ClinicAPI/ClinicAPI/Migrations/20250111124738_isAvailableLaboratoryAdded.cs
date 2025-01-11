using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicAPI.Migrations
{
    /// <inheritdoc />
    public partial class isAvailableLaboratoryAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date",
                table: "LaboratoryAppointment",
                newName: "DateTime");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "LaboratoryWorker",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "LaboratorySupervisor",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "LaboratoryWorker");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "LaboratorySupervisor");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "LaboratoryAppointment",
                newName: "date");
        }
    }
}
