using System.Security.Claims;

namespace ClinicAPI.UserFeatures
{
    public interface IUserContext
    {
        //CurrentUser? GetCurrentUser();
        ClaimsPrincipal GetCurrentUser();
        string? GetCurrentUserId();
        string? GetCurrentUserEmail();
        public string? GetCurrentUserRole();
    }

    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public string? GetCurrentUserId()
        {
            return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public string? GetCurrentUserEmail()
        {
            return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
        }

        public string? GetCurrentUserRole() //NIE WIEM CZY NIE CALA LISTA?
        {
            return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
        }

        //public CurrentUser? GetCurrentUser()
        public ClaimsPrincipal GetCurrentUser()
        {
            var user = httpContextAccessor?.HttpContext?.User;
            if (user == null)
            {
                throw new InvalidOperationException("User context is not present");
            }
            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }
            var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value);

            //wersja2:
            /*var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);*/

            //return new CurrentUser(userId, email, roles); //email! z wykrzyknikiem
            return user; //email! z wykrzyknikiem
        }
    }
}
