using JPProject.Sso.Domain.Commands.UserManagement;

namespace JPProject.Sso.Domain.Validations.UserManagement
{
    public class UpdateUserCommandValidation : UserManagementValidation<UpdateUserCommand>
    {
        public UpdateUserCommandValidation()
        {
            ValidateEmail();
            ValidateName();
            ValidateUsername();
        }

    }
}