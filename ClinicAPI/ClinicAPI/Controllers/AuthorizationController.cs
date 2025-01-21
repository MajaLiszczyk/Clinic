using ClinicAPI.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens; // SymmetricSecurityKey, SigningCredentials, SecurityAlgorithms
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization; // JwtSecurityToken, JwtSecurityTokenHandler
using ClinicAPI.DB;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Controllers
{
    public class UserLoginRequest
    {
        [Required]
        public string Email { get; set; } = default!;
        [Required]
        public string Password { get; set; } = default!;
    }

    [ApiController]
    [Route("api/authorization/")]
    public class AuthorizationController : ControllerBase
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly ApplicationDBContext _dbContext;

        public AuthorizationController(SignInManager<User> signInManager, UserManager<User> userManager, ApplicationDBContext dbContext)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _dbContext = dbContext;
        }



        /*Metoda login zwraca obiekt JSON:
        {
            "Token": "string",   // JWT token
            "Role": "string",    // Rola użytkownika, np. "Patient" lub "Doctor"
            "UserId": "string",  // Unikalny identyfikator użytkownika
            "Id": 0              // Id pacjenta lub lekarza (jeśli rola to "Patient" lub "Doctor"), inaczej 0
        } */
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            //var result = await signInManager.PasswordSignInAsync(user.UserName, request.Password, isPersistent: false, lockoutOnFailure: false);
            var result = await userManager.CheckPasswordAsync(user, request.Password);//wersja2
            //if (!result.Succeeded)
            if (!result) //wersja 2
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            var roles = await userManager.GetRolesAsync(user);
            string role = roles.FirstOrDefault() ?? "Unknown";
            int id = role switch
            {
                "Patient" => _dbContext.Patient.FirstOrDefault(p => p.UserId == user.Id)?.Id ?? 0,
                "Doctor" => _dbContext.Doctor.FirstOrDefault(d => d.UserId == user.Id)?.Id ?? 0,
                "LaboratoryWorker" => _dbContext.LaboratoryWorker.FirstOrDefault(d => d.UserId == user.Id)?.Id ?? 0,
                "LaboratorySupervisor" => _dbContext.LaboratorySupervisor.FirstOrDefault(d => d.UserId == user.Id)?.Id ?? 0,
                _ => 0
            };

            var token = GenerateJwtToken(user, roles);

            return Ok(new
            {
                Token = token,
                //Roles = roles,
                Role = role,
                UserId = user.Id,
                Id = id
                //Message = "Login successful"
            });
        }



        private string GenerateJwtToken(User user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique token ID // ??
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //alternatyw:
            //new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "User")
            // Dodanie niestandardowego claimu
            /*if (user.PatientId != null)
            {
                claims.Add(new Claim("patientId", user.PatientId.ToString()));
            }*/

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsA32CharacterLongSecretKey!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //po co?
            var token = new JwtSecurityToken(
                issuer: "your_issuer",
                audience: "your_audience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //TESTOWA, DO WYRZUCENIA POTEM
        [HttpGet("auth")]
        [Authorize]
        public IActionResult GetAuthenticated()
        {
            return Ok(new { Message = "You are authenticated" });
        }

    }


}
