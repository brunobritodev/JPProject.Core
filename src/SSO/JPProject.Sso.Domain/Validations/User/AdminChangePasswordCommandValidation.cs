using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class AdminChangePasswordCommandValidation : UserValidation<AdminChangePasswordCommand>
    {
        public AdminChangePasswordCommandValidation()
        {
            ValidateUsername();
        }
    }
}