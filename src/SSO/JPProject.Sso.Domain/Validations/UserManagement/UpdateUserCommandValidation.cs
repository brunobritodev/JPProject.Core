using JPProject.Sso.Domain.Commands.UserManagement;

namespace JPProject.Sso.Domain.Validations.UserManagement
{
    public class UpdateUserCommandValidation : UserManagementValidation<UserManagementCommand>
    {
        public UpdateUserCommandValidation()
        {
            ValidateEmail();
            ValidateName();
            ValidateUsername();
        }

    }
}