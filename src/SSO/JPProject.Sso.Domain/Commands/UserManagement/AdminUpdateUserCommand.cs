using JPProject.Sso.Domain.Validations.UserManagement;
using System;

namespace JPProject.Sso.Domain.Commands.UserManagement
{
    public class AdminUpdateUserCommand : UserManagementCommand
    {

        public AdminUpdateUserCommand(string email, string userName, string name, string phoneNumber, bool emailConfirmed, bool phoneNumberConfirmed, bool twoFactorEnabled, DateTimeOffset? lockoutEnd, bool lockoutEnabled, int accessFailedCount, DateTime? birthdate, string ssn)
        {
            Birthdate = birthdate;
            SocialNumber = ssn;
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
        }


        public override bool IsValid()
        {
            ValidationResult = new UpdateUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
