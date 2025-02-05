using ClinicAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClinicAPI.DB
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.MedicalSpecialisations)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "DoctorMedicalSpecialisation",
                    j => j.HasOne<MedicalSpecialisation>().WithMany().HasForeignKey("MedicalSpecialisationId"),
                    j => j.HasOne<Doctor>().WithMany().HasForeignKey("DoctorId")
                );
            modelBuilder.Entity<User>().Property(u => u.TestString).HasMaxLength(5);

            modelBuilder.Entity<Admin>()
            .HasOne(a => a.User)
            .WithOne()
            .HasForeignKey<Admin>(a => a.UserId)
            .IsRequired(false);

             modelBuilder.Entity<Doctor>()
            .HasOne(a => a.User)
            .WithOne() 
            .HasForeignKey<Doctor>(a => a.UserId)
            .IsRequired(false);

             modelBuilder.Entity<LaboratorySupervisor>()
            .HasOne(a => a.User)
            .WithOne()
            .HasForeignKey<LaboratorySupervisor>(a => a.UserId)
            .IsRequired(false);

             modelBuilder.Entity<LaboratoryWorker>()
            .HasOne(a => a.User)
            .WithOne()
            .HasForeignKey<LaboratoryWorker>(a => a.UserId)
            .IsRequired(false);

             modelBuilder.Entity<Patient>()
            .HasOne(a => a.User)
            .WithOne()
            .HasForeignKey<Patient>(a => a.UserId)
            .IsRequired(false);

             modelBuilder.Entity<Registrant>()
            .HasOne(a => a.User)
            .WithOne() 
            .HasForeignKey<Registrant>(a => a.UserId)
            .IsRequired(false);
        }

        public DbSet<MedicalSpecialisation> MedicalSpecialisations { get; set; }
        public DbSet<Admin> Admin {  get; set; }
        public DbSet<DiagnosticTest> DiagnosticTest {  get; set; }
        public DbSet<Doctor> Doctor {  get; set; }
        public DbSet<LaboratorySupervisor> LaboratorySupervisor {  get; set; }
        public DbSet<LaboratoryWorker> LaboratoryWorker {  get; set; }
        public DbSet<LaboratoryTest> LaboratoryTest {  get; set; }
        public DbSet<MedicalAppointment> MedicalAppointment {  get; set; }
        public DbSet<Patient> Patient {  get; set; }
        public DbSet<Registrant> Registrant {  get; set; }
        public DbSet<MedicalSpecialisation> MedicalSpecialisation {  get; set; }
        public DbSet<DiagnosticTestType> DiagnosticTestType {  get; set; }
        public DbSet<LaboratoryTestType> LaboratoryTestType {  get; set; }
        public DbSet<LaboratoryAppointment> LaboratoryAppointment {  get; set; }
        public DbSet<LaboratoryTestsGroup> LaboratoryTestsGroup {  get; set; }
    }
}
