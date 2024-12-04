using ClinicAPI.Models;
using ClinicAPI.Models.ApplicationUsers;
using Microsoft.EntityFrameworkCore;

namespace ClinicAPI.DB
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // EF Core automatycznie utworzy tabelę pośredniczącą dla relacji wiele do wielu
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.MedicalSpecialisations)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "DoctorMedicalSpecialisation", // Nazwa tabeli pośredniczącej
                    j => j.HasOne<MedicalSpecialisation>().WithMany().HasForeignKey("MedicalSpecialisationId"),
                    j => j.HasOne<Doctor>().WithMany().HasForeignKey("DoctorId")
                );
        }



        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.MedicalSpecialisations)
                .WithMany()
                .UsingEntity(j => j.ToTable("DoctorMedicalSpecialisations")); // Tabela pośrednicząca
        }*/

        public DbSet<MedicalSpecialisation> MedicalSpecialisations { get; set; }
        public DbSet<ApplicationUser> ApplicationUser {  get; set; }
        public DbSet<ApplicationUserAdmin> ApplicationUserAdmin {  get; set; }
        public DbSet<ApplicationUserDoctor> ApplicationUserDoctor {  get; set; }
        public DbSet<ApplicationUserLaboratorySupervisor> ApplicationUserLaboratorySupervisor {  get; set; }
        public DbSet<ApplicationUserLaboratoryWorker> ApplicationUserLaboratoryWorker {  get; set; }
        public DbSet<ApplicationUserPatient> ApplicationUserPatient {  get; set; }
        public DbSet<ApplicationUserRegistrant> ApplicationUserRegistrant {  get; set; }
        public DbSet<Admin> Admin {  get; set; }
        public DbSet<DiagnosticTest> DiagnosticTest {  get; set; }
        public DbSet<Doctor> Doctor {  get; set; }
        public DbSet<LaboratorySupervisor> LaboratorySupervisor {  get; set; }
        public DbSet<LaboratoryWorker> LaboratoryWorker {  get; set; }
        public DbSet<LaboratoryTest> LaboratoryTest {  get; set; }
        public DbSet<MedicalAppointment> MedicalAppointment {  get; set; }
        public DbSet<Patient> Patient {  get; set; }
        public DbSet<Registrant> Registrant {  get; set; }
        public DbSet<Role> Role {  get; set; }
        public DbSet<MedicalSpecialisation> MedicalSpecialisation {  get; set; }
    }
}
