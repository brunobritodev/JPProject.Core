using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class SaveUserRoleCommandValidation : UserRoleValidation<SaveUserRoleCommand>
    {
        public SaveUserRoleCommandValidation()
        {
            ValidateUsername();
            ValidateRole();
        }
    }
}