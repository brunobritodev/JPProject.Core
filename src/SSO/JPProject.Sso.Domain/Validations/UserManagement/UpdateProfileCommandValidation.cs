using JPProject.Sso.Domain.Commands.UserManagement;

namespace JPProject.Sso.Domain.Validations.UserManagement
{
    public class UpdateProfileCommandValidation : ProfileValidation<ProfileCommand>
    {
        public UpdateProfileCommandValidation()
        {
            ValidateUsername();
        }
    }
}
