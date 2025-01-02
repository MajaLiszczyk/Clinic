using MediatR;
namespace ClinicAPI.UserFeatures.Commands.UpdateUserDetails
{
    public class UpdateUserDetailsCommand : IRequest
    {
        public string? testString { get; set; }
    }
}
