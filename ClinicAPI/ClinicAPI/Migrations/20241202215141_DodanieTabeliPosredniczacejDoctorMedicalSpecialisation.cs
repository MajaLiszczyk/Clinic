using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClinicAPI.Migrations
{
    /// <inheritdoc />
    public partial class DodanieTabeliPosredniczacejDoctorMedicalSpecialisation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PatientNumber",
                table: "Patient",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LaboratoryWorkerNumber",
                table: "LaboratoryWorker",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LaboratorySupervisorNumber",
                table: "LaboratorySupervisor",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DoctorNumber",
                table: "Doctor",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MedicalSpecialisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalSpecialisation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Registrant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    RegistrantNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorMedicalSpecialisation",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "integer", nullable: false),
                    MedicalSpecialisationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorMedicalSpecialisation", x => new { x.DoctorId, x.MedicalSpecialisationId });
                    table.ForeignKey(
                        name: "FK_DoctorMedicalSpecialisation_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorMedicalSpecialisation_MedicalSpecialisation_MedicalSp~",
                        column: x => x.MedicalSpecialisationId,
                        principalTable: "MedicalSpecialisation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorMedicalSpecialisation_MedicalSpecialisationId",
                table: "DoctorMedicalSpecialisation",
                column: "MedicalSpecialisationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorMedicalSpecialisation");

            migrationBuilder.DropTable(
                name: "Registrant");

            migrationBuilder.DropTable(
                name: "MedicalSpecialisation");

            migrationBuilder.DropColumn(
                name: "PatientNumber",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "LaboratoryWorkerNumber",
                table: "LaboratoryWorker");

            migrationBuilder.DropColumn(
                name: "LaboratorySupervisorNumber",
                table: "LaboratorySupervisor");

            migrationBuilder.DropColumn(
                name: "DoctorNumber",
                table: "Doctor");
        }
    }
}
