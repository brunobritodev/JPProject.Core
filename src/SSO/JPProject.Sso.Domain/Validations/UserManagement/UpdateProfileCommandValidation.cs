using JPProject.Sso.Domain.Commands.UserManagement;

namespace JPProject.Sso.Domain.Validations.UserManagement
{
    public class UpdateProfileCommandValidation : ProfileValidation<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidation()
        {
            ValidateId();
            ValidateName();
        }
    }
}
