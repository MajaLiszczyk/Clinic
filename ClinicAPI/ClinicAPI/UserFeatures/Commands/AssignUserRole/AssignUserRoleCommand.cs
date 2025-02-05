using MediatR;

namespace ClinicAPI.UserFeatures.Commands.AssignUserRole; 

public class AssignUserRoleCommand : IRequest
{
    public string UserEmail { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}
