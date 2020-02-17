using System;
using JPProject.Sso.Domain.Validations.User;

namespace JPProject.Sso.Domain.Commands.User
{
    public class RegisterNewUserWithProviderCommand : UserCommand
    {
        public RegisterNewUserWithProviderCommand(string username, string email, string name, string phoneNumber, string password, string confirmPassword, string picture, string provider, string providerId, DateTime? birthdate, string socialNumber)
        {
            Birthdate = birthdate;
            SocialNumber = socialNumber;
            Picture = picture;
            Provider = provider;
            ProviderId = providerId;
            Username = username;
            Email = email;
            Name = name;
            PhoneNumber = phoneNumber;
            Password = password;
            ConfirmPassword = confirmPassword;
        }


        public override bool IsValid()
        {
            ValidationResult = new RegisterNewUserWithProviderCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}