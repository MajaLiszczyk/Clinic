using ClinicAPI.Models;
using ClinicAPI.UserFeatures;

namespace ClinicAPI.Authorization.Services;

public class ClinicAuthorizationService(ILogger<ClinicAuthorizationService> logger, IUserContext userContext)
{
    public bool Authorize(DiagnosticTestType diagnosticTestType, ResoureOperation resoureOperation)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for diagnostic test type {DiagnosticTestType}"
                              ,user.Email, resoureOperation, diagnosticTestType.Name);
        if(resoureOperation == ResoureOperation.Read || resoureOperation == ResoureOperation.Create)
        {
            logger.LogInformation("Create/read operation - successful authorization");
            return true;
        }
        if(resoureOperation == ResoureOperation.Delete && user.IsInRole(UserRole.Admin))
        {
            logger.LogInformation("Admin user, delete operation - successful authorization");
            return true;
        }
        if (resoureOperation == ResoureOperation.Delete || resoureOperation == ResoureOperation.Update)
        {
            logger.LogInformation("Admin user, delete operation - successful authorization");
            return true;

        }
        return false;

    }
    //jest werufikacją czy podany user z contextu powinien zając się daną operacją, czy nie
    public bool AuthorizeMedicalAppointment(DiagnosticTestType diagnosticTestType, ResoureOperation resoureOperation)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for diagnostic test type {DiagnosticTestType}"
                              , user.Email, resoureOperation, diagnosticTestType.Name);
        if (resoureOperation == ResoureOperation.Read || resoureOperation == ResoureOperation.Create)
        {
            logger.LogInformation("Create/read operation - successful authorization");
            return true;
        }
        if (resoureOperation == ResoureOperation.Delete && user.IsInRole(UserRole.Admin))
        {
            logger.LogInformation("Admin user, delete operation - successful authorization");
            return true;
        }
        if (resoureOperation == ResoureOperation.Delete || resoureOperation == ResoureOperation.Update)
        {
            logger.LogInformation("Admin user, delete operation - successful authorization");
            return true;

        }
        return false;

    }

}
