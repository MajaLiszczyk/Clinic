using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClinicAPI.Migrations
{
    /// <inheritdoc />
    public partial class LaboratoryPartModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LaboratoryWorkerId",
                table: "LaboratoryTest");

            migrationBuilder.DropColumn(
                name: "date",
                table: "LaboratoryTest");

            migrationBuilder.RenameColumn(
                name: "SupervisorId",
                table: "LaboratoryTest",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "MedicalAppointmentId",
                table: "LaboratoryTest",
                newName: "LaboratoryTestsGroupId");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorNote",
                table: "LaboratoryTest",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "RejectComment",
                table: "LaboratoryTest",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "wynik",
                table: "LaboratoryTest",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LaboratoryAppointment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LaboratoryWorkerId = table.Column<int>(type: "integer", nullable: false),
                    SupervisorId = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CancelComment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaboratoryAppointment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaboratoryTestsGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MedicalAppointmentId = table.Column<int>(type: "integer", nullable: false),
                    LaboratoryAppointmentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaboratoryTestsGroup", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LaboratoryAppointment");

            migrationBuilder.DropTable(
                name: "LaboratoryTestsGroup");

            migrationBuilder.DropColumn(
                name: "RejectComment",
                table: "LaboratoryTest");

            migrationBuilder.DropColumn(
                name: "wynik",
                table: "LaboratoryTest");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "LaboratoryTest",
                newName: "SupervisorId");

            migrationBuilder.RenameColumn(
                name: "LaboratoryTestsGroupId",
                table: "LaboratoryTest",
                newName: "MedicalAppointmentId");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorNote",
                table: "LaboratoryTest",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LaboratoryWorkerId",
                table: "LaboratoryTest",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "LaboratoryTest",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
