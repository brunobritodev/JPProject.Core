using JPProject.Sso.Domain.Validations.User;
using System;

namespace JPProject.Sso.Domain.Commands.User
{
    public class RegisterNewUserCommand : UserCommand
    {

        public RegisterNewUserCommand(string username, string email, string name, string phoneNumber, string password, string confirmPassword, DateTime birthdate, string socialNumber)
        {
            Birthdate = birthdate;
            SocialNumber = socialNumber;
            Username = username;
            Email = email;
            Name = name;
            PhoneNumber = phoneNumber;
            Password = password;
            ConfirmPassword = confirmPassword;

        }
        public override bool IsValid()
        {
            ValidationResult = new RegisterNewUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
