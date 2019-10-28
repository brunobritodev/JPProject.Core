using System;
using JPProject.Sso.Domain.Validations.UserManagement;

namespace JPProject.Sso.Domain.Commands.UserManagement
{
    public class ChangePasswordCommand : PasswordCommand
    {

        public ChangePasswordCommand(Guid? id, string oldPassword, string newPassword, string confirmPassword)
        {
            Id = id;
            OldPassword = oldPassword;
            Password = newPassword;
            ConfirmPassword = confirmPassword;
        }

        public override bool IsValid()
        {
            ValidationResult = new ChangePasswordCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}