using ClinicAPI.Models;
using ClinicAPI.UserFeatures.Commands.UpdateUserDetails;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ClinicAPI.UserFeatures.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(ILogger<AssignUserRoleCommandHandler> logger
                                         ,UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : IRequestHandler<AssignUserRoleCommand>
{
    public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Assigning user role: {@Request}", request);
        var user = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new Exception("User not found");
        var role = await roleManager.FindByNameAsync(request.RoleName)
            ?? throw new Exception("Role not found");
        await userManager.AddToRoleAsync(user, role.Name!);
    }
}

