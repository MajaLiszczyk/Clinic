using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClinicAPI.Migrations
{
    /// <inheritdoc />
    public partial class ConnectedUserAndActors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "ApplicationUserAdmin");

            migrationBuilder.DropTable(
                name: "ApplicationUserDoctor");

            migrationBuilder.DropTable(
                name: "ApplicationUserLaboratorySupervisor");

            migrationBuilder.DropTable(
                name: "ApplicationUserLaboratoryWorker");

            migrationBuilder.DropTable(
                name: "ApplicationUserPatient");

            migrationBuilder.DropTable(
                name: "ApplicationUserRegistrant");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Registrant",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LaboratoryWorker",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LaboratorySupervisor",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Doctor",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Admin",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Admin",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registrant_UserId",
                table: "Registrant",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patient_UserId",
                table: "Patient",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryWorker_UserId",
                table: "LaboratoryWorker",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaboratorySupervisor_UserId",
                table: "LaboratorySupervisor",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_UserId",
                table: "Doctor",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admin_AppUserId",
                table: "Admin",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Admin_UserId",
                table: "Admin",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_AspNetUsers_AppUserId",
                table: "Admin",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_AspNetUsers_UserId",
                table: "Admin",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_AspNetUsers_UserId",
                table: "Doctor",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratorySupervisor_AspNetUsers_UserId",
                table: "LaboratorySupervisor",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryWorker_AspNetUsers_UserId",
                table: "LaboratoryWorker",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_AspNetUsers_UserId",
                table: "Patient",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Registrant_AspNetUsers_UserId",
                table: "Registrant",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_AspNetUsers_AppUserId",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_Admin_AspNetUsers_UserId",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_AspNetUsers_UserId",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_LaboratorySupervisor_AspNetUsers_UserId",
                table: "LaboratorySupervisor");

            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryWorker_AspNetUsers_UserId",
                table: "LaboratoryWorker");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_AspNetUsers_UserId",
                table: "Patient");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrant_AspNetUsers_UserId",
                table: "Registrant");

            migrationBuilder.DropIndex(
                name: "IX_Registrant_UserId",
                table: "Registrant");

            migrationBuilder.DropIndex(
                name: "IX_Patient_UserId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_LaboratoryWorker_UserId",
                table: "LaboratoryWorker");

            migrationBuilder.DropIndex(
                name: "IX_LaboratorySupervisor_UserId",
                table: "LaboratorySupervisor");

            migrationBuilder.DropIndex(
                name: "IX_Doctor_UserId",
                table: "Doctor");

            migrationBuilder.DropIndex(
                name: "IX_Admin_AppUserId",
                table: "Admin");

            migrationBuilder.DropIndex(
                name: "IX_Admin_UserId",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Registrant");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LaboratoryWorker");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LaboratorySupervisor");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Admin");

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Login = table.Column<int>(type: "integer", nullable: false),
                    Password = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserAdmin",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserAdmin", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserDoctor",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserDoctor", x => x.DoctorId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserLaboratorySupervisor",
                columns: table => new
                {
                    LaboratorySupervisorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserLaboratorySupervisor", x => x.LaboratorySupervisorId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserLaboratoryWorker",
                columns: table => new
                {
                    LaboratoryWorkerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserLaboratoryWorker", x => x.LaboratoryWorkerId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserPatient",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserPatient", x => x.PatientId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserRegistrant",
                columns: table => new
                {
                    RegistrantId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRegistrant", x => x.RegistrantId);
                });
        }
    }
}
