using ClinicAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ClinicAPI.Authorization
{
    public class ClinicUserClaimsPrincipalFactory(UserManager<User> userManager
                 ,RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) 
                 : UserClaimsPrincipalFactory<User, IdentityRole>(userManager, roleManager, options)
    {
        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var id = await  GenerateClaimsAsync(user);

            if(user.TestString != null)
            {
                id.AddClaim(new Claim("TestString", user.TestString));
            }
            return new ClaimsPrincipal(id);
        }
    }
}
