using JPProject.Sso.Domain.Validations.User;
using System;

namespace JPProject.Sso.Domain.Commands.User
{
    public class RegisterNewUserCommand : UserCommand
    {
        public RegisterNewUserCommand(string username, string email, string name, string phoneNumber, string password, string confirmPassword, DateTime? birthdate, string ssn, bool shouldConfirmEmail = false)
        {
            Birthdate = birthdate;
            Username = username;
            Email = email;
            Name = name;
            PhoneNumber = phoneNumber;
            Password = password;
            ConfirmPassword = confirmPassword;
            SocialNumber = ssn;
            EmailConfirmed = !shouldConfirmEmail;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}
