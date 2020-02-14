using System;
using JPProject.Sso.Domain.Validations.UserManagement;

namespace JPProject.Sso.Domain.Commands.UserManagement
{
    public class UpdateUserCommand : UserManagementCommand
    {

        public UpdateUserCommand(string email, string userName, string name, string phoneNumber, bool emailConfirmed, bool phoneNumberConfirmed, bool twoFactorEnabled, DateTimeOffset? lockoutEnd, bool lockoutEnabled, int accessFailedCount, string socialNumber, DateTime? birthDate)
        {
            Email = email;
            Username = userName;
            EmailConfirmed = emailConfirmed;
            PhoneNumberConfirmed = phoneNumberConfirmed;
            TwoFactorEnabled = twoFactorEnabled;
            LockoutEnd = lockoutEnd;
            LockoutEnabled = lockoutEnabled;
            AccessFailedCount = accessFailedCount;
            Name = name;
            PhoneNumber = phoneNumber;
            SocialNumber = socialNumber;
            Birthdate = birthDate;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
