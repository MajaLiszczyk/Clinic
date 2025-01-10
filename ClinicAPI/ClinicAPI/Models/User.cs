using Microsoft.AspNetCore.Identity;

namespace ClinicAPI.Models
{
    public class User : IdentityUser
    {
        public string? TestString { get; set; } 
    }
}
