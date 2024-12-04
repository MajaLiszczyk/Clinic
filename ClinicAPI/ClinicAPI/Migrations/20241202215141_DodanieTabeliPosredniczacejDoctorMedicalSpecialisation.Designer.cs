﻿// <auto-generated />
using System;
using ClinicAPI.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClinicAPI.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20241202215141_DodanieTabeliPosredniczacejDoctorMedicalSpecialisation")]
    partial class DodanieTabeliPosredniczacejDoctorMedicalSpecialisation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ClinicAPI.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("ClinicAPI.Models.ApplicationUsers.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Login")
                        .HasColumnType("integer");

                    b.Property<int>("Password")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUser");
                });

            modelBuilder.Entity("ClinicAPI.Models.ApplicationUsers.ApplicationUserAdmin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AdminId"));

                    b.Property<int>("ApplicationUserId")
                        .HasColumnType("integer");

                    b.HasKey("AdminId");

                    b.ToTable("ApplicationUserAdmin");
                });

            modelBuilder.Entity("ClinicAPI.Models.ApplicationUsers.ApplicationUserDoctor", b =>
                {
                    b.Property<int>("DoctorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DoctorId"));

                    b.Property<int>("ApplicationUserId")
                        .HasColumnType("integer");

                    b.HasKey("DoctorId");

                    b.ToTable("ApplicationUserDoctor");
                });

            modelBuilder.Entity("ClinicAPI.Models.ApplicationUsers.ApplicationUserLaboratorySupervisor", b =>
                {
                    b.Property<int>("LaboratorySupervisorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LaboratorySupervisorId"));

                    b.Property<int>("ApplicationUserId")
                        .HasColumnType("integer");

                    b.HasKey("LaboratorySupervisorId");

                    b.ToTable("ApplicationUserLaboratorySupervisor");
                });

            modelBuilder.Entity("ClinicAPI.Models.ApplicationUsers.ApplicationUserLaboratoryWorker", b =>
                {
                    b.Property<int>("LaboratoryWorkerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LaboratoryWorkerId"));

                    b.Property<int>("ApplicationUserId")
                        .HasColumnType("integer");

                    b.HasKey("LaboratoryWorkerId");

                    b.ToTable("ApplicationUserLaboratoryWorker");
                });

            modelBuilder.Entity("ClinicAPI.Models.ApplicationUsers.ApplicationUserPatient", b =>
                {
                    b.Property<int>("PatientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PatientId"));

                    b.Property<int>("ApplicationUserId")
                        .HasColumnType("integer");

                    b.HasKey("PatientId");

                    b.ToTable("ApplicationUserPatient");
                });

            modelBuilder.Entity("ClinicAPI.Models.ApplicationUsers.ApplicationUserRegistrant", b =>
                {
                    b.Property<int>("RegistrantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RegistrantId"));

                    b.Property<int>("ApplicationUserId")
                        .HasColumnType("integer");

                    b.HasKey("RegistrantId");

                    b.ToTable("ApplicationUserRegistrant");
                });

            modelBuilder.Entity("ClinicAPI.Models.DiagnosticTest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("DoctorId")
                        .HasColumnType("integer");

                    b.Property<int>("MedicalAppoitmentId")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("date")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("DiagnosticTest");
                });

            modelBuilder.Entity("ClinicAPI.Models.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DoctorNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Doctor");
                });

            modelBuilder.Entity("ClinicAPI.Models.LaboratorySupervisor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("LaboratorySupervisorNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("LaboratorySupervisor");
                });

            modelBuilder.Entity("ClinicAPI.Models.LaboratoryTest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DoctorNote")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("LaboratoryWorkerId")
                        .HasColumnType("integer");

                    b.Property<int>("MedicalAppointmentId")
                        .HasColumnType("integer");

                    b.Property<int>("SupervisorId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("laboratoryTestType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("LaboratoryTest");
                });

            modelBuilder.Entity("ClinicAPI.Models.LaboratoryWorker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("LaboratoryWorkerNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("LaboratoryWorker");
                });

            modelBuilder.Entity("ClinicAPI.Models.MedicalAppointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Diagnosis")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("DiseaseUnit")
                        .HasColumnType("integer");

                    b.Property<int>("DoctorId")
                        .HasColumnType("integer");

                    b.Property<string>("Interview")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PatientId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("dateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("MedicalAppointment");
                });

            modelBuilder.Entity("ClinicAPI.Models.MedicalSpecialisation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MedicalSpecialisation");
                });

            modelBuilder.Entity("ClinicAPI.Models.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PatientNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Pesel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Patient");
                });

            modelBuilder.Entity("ClinicAPI.Models.Registrant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RegistrantNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Registrant");
                });

            modelBuilder.Entity("ClinicAPI.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("DoctorMedicalSpecialisation", b =>
                {
                    b.Property<int>("DoctorId")
                        .HasColumnType("integer");

                    b.Property<int>("MedicalSpecialisationId")
                        .HasColumnType("integer");

                    b.HasKey("DoctorId", "MedicalSpecialisationId");

                    b.HasIndex("MedicalSpecialisationId");

                    b.ToTable("DoctorMedicalSpecialisation");
                });

            modelBuilder.Entity("DoctorMedicalSpecialisation", b =>
                {
                    b.HasOne("ClinicAPI.Models.Doctor", null)
                        .WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicAPI.Models.MedicalSpecialisation", null)
                        .WithMany()
                        .HasForeignKey("MedicalSpecialisationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
