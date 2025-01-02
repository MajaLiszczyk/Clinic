using ClinicAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace ClinicAPI.UserFeatures.Commands.UpdateUserDetails
{
    public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> logger, IUserContext userContext
        , IUserStore<User> userStore) : IRequestHandler<UpdateUserDetailsCommand>
    {
        public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();
            logger.LogInformation("Updating user: {UserId}, with {@Request}", user!.Id, request);

            var dbUser = await userStore.FindByIdAsync(user!.Id, cancellationToken);

            if (dbUser == null)
            {
                throw new Exception("There is nno this user");
                //throw new NotFoundException(nameof(User), user!.Id); //on tak ma, ale to jego wlasna klasa wyjykow
            }
            dbUser.TestString = request.testString;

            await userStore.UpdateAsync(dbUser, cancellationToken);
        }


    }
}
