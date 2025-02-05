using ClinicAPI.DB;
using ClinicAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace ClinicAPI.Seeders
{
    internal class ClinicSeeder : IClinicSeeder
    {

        private readonly ApplicationDBContext dbContext;
        private readonly UserManager<User> _userManager;


        public ClinicSeeder(ApplicationDBContext ddbContext, UserManager<User> userManager)
        {
            dbContext = ddbContext ?? throw new ArgumentNullException(nameof(ddbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        }

        private IEnumerable<IdentityRole> GetRoles()
        {
            List<IdentityRole> roles =
                [
                    new(UserRole.Admin)
                    {
                        NormalizedName = UserRole.Admin.ToUpper()
                    },
                    new(UserRole.Patient)
                    {
                        NormalizedName = UserRole.Patient.ToUpper()
                    },
                    new(UserRole.Doctor)
                    {
                        NormalizedName = UserRole.Doctor.ToUpper()
                    },
                    new(UserRole.Registrant)
                    {
                        NormalizedName = UserRole.Registrant.ToUpper()
                    },
                    new(UserRole.LaboratoryWorker)
                    {
                        NormalizedName = UserRole.LaboratoryWorker.ToUpper()
                    },
                    new(UserRole.LaboratorySupervisor)
                    {
                        NormalizedName = UserRole.LaboratorySupervisor.ToUpper()
                    },

                ];
            return roles;
        }

        public async Task Seed()
        {
            if(!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
            var admin = new User
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(admin, "Admin123!");
            var addToRoleResult = await _userManager.AddToRoleAsync(admin, UserRole.Admin);
        }
    }
}
