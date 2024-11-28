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
    }
}
