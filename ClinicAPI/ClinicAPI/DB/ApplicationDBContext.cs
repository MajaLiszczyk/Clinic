using ClinicAPI.Models;
//using ClinicAPI.Models.ApplicationUsers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClinicAPI.DB
{
    public class ApplicationDBContext : IdentityDbContext<User> // wczesniej dziedziczylam po zwykłym DbContext, ale ten Identity też gdzieś tam po nim dziedziczy
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
            modelBuilder.Entity<User>().Property(u => u.TestString).HasMaxLength(5);
            //modelBuilder.HasDefaultSchema("identity"); //pAN YOUTUBE ANG TAK ZROBIŁ

            modelBuilder.Entity<Admin>()
            .HasOne(a => a.User)
            .WithOne() // Usuwamy tutaj nawigację w User
            .HasForeignKey<Admin>(a => a.UserId)
            .IsRequired(false); // Relacja jest opcjonalna

             modelBuilder.Entity<Doctor>()
            .HasOne(a => a.User)
            .WithOne() // Usuwamy tutaj nawigację w User
            .HasForeignKey<Doctor>(a => a.UserId)
            .IsRequired(false); // Relacja jest opcjonalna

             modelBuilder.Entity<LaboratorySupervisor>()
            .HasOne(a => a.User)
            .WithOne() // Usuwamy tutaj nawigację w User
            .HasForeignKey<LaboratorySupervisor>(a => a.UserId)
            .IsRequired(false); // Relacja jest opcjonalna

             modelBuilder.Entity<LaboratoryWorker>()
            .HasOne(a => a.User)
            .WithOne() // Usuwamy tutaj nawigację w User
            .HasForeignKey<LaboratoryWorker>(a => a.UserId)
            .IsRequired(false); // Relacja jest opcjonalna

             modelBuilder.Entity<Patient>()
            .HasOne(a => a.User)
            .WithOne() // Usuwamy tutaj nawigację w User
            .HasForeignKey<Patient>(a => a.UserId)
            .IsRequired(false); // Relacja jest opcjonalna

             modelBuilder.Entity<Registrant>()
            .HasOne(a => a.User)
            .WithOne() // Usuwamy tutaj nawigację w User
            .HasForeignKey<Registrant>(a => a.UserId)
            .IsRequired(false); // Relacja jest opcjonalna
        }



        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.MedicalSpecialisations)
                .WithMany()
                .UsingEntity(j => j.ToTable("DoctorMedicalSpecialisations")); // Tabela pośrednicząca
        }*/

        public DbSet<MedicalSpecialisation> MedicalSpecialisations { get; set; }
        /*public DbSet<ApplicationUser> ApplicationUser {  get; set; }
        public DbSet<ApplicationUserAdmin> ApplicationUserAdmin {  get; set; }
        public DbSet<ApplicationUserDoctor> ApplicationUserDoctor {  get; set; }
        public DbSet<ApplicationUserLaboratorySupervisor> ApplicationUserLaboratorySupervisor {  get; set; }
        public DbSet<ApplicationUserLaboratoryWorker> ApplicationUserLaboratoryWorker {  get; set; }
        public DbSet<ApplicationUserPatient> ApplicationUserPatient {  get; set; }
        public DbSet<ApplicationUserRegistrant> ApplicationUserRegistrant {  get; set; } */
        public DbSet<Admin> Admin {  get; set; }
        public DbSet<DiagnosticTest> DiagnosticTest {  get; set; }
        public DbSet<Doctor> Doctor {  get; set; }
        public DbSet<LaboratorySupervisor> LaboratorySupervisor {  get; set; }
        public DbSet<LaboratoryWorker> LaboratoryWorker {  get; set; }
        public DbSet<LaboratoryTest> LaboratoryTest {  get; set; }
        public DbSet<MedicalAppointment> MedicalAppointment {  get; set; }
        public DbSet<Patient> Patient {  get; set; }
        public DbSet<Registrant> Registrant {  get; set; }
        //public DbSet<Role> Role {  get; set; }
        public DbSet<MedicalSpecialisation> MedicalSpecialisation {  get; set; }
        public DbSet<DiagnosticTestType> DiagnosticTestType {  get; set; }
        public DbSet<LaboratoryTestType> LaboratoryTestType {  get; set; }
    }
}
