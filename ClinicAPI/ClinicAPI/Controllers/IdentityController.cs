using ClinicAPI.Models;
using ClinicAPI.UserFeatures.Commands.AssignUserRole;
using ClinicAPI.UserFeatures.Commands.UpdateUserDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/identity/")]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
        [HttpPatch("user")] //ten endpoint ma sens tylko dla uwierzytelnionych użytkowników, bo dla innego nie istnieje context
        [Authorize]
        public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
        {
            await mediator.Send(command);
            return NoContent();

        }

        [HttpPost("userRole")] //ten endpoint ma sens tylko dla uwierzytelnionych użytkowników, bo dla innego nie istnieje context
        [Authorize]
        public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
        {
            await mediator.Send(command);
            return NoContent();

        }

    }
}
