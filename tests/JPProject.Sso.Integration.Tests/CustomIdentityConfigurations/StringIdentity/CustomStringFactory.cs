using JPProject.Sso.Domain.Commands.User;
using JPProject.Sso.Domain.Commands.UserManagement;
using JPProject.Sso.Domain.Interfaces;
using System;

namespace JPProject.Sso.Integration.Tests.CustomIdentityConfigurations.StringIdentity
{
    public class CustomStringFactory : IIdentityFactory<CustomStringIdentity>, IRoleFactory<CustomRoleStringIdentity>
    {
        public CustomStringIdentity Create(UserCommand user)
        {

            return new CustomStringIdentity
            {
                Id = Guid.NewGuid().ToString(),
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserName = user.Username,
                Name = user.Name,
                Picture = user.Picture,
                EmailConfirmed = user.EmailConfirmed,
                SocialNumber = user.SocialNumber,
                Birthdate = user.Birthdate,
                LockoutEnd = null,
            };
        }

        public CustomRoleStringIdentity CreateRole(string name)
        {
            return new CustomRoleStringIdentity(name);
        }


        public void UpdateInfo(AdminUpdateUserCommand command, CustomStringIdentity stringDb)
        {
            stringDb.Email = command.Email;
            stringDb.EmailConfirmed = command.EmailConfirmed;
            stringDb.AccessFailedCount = command.AccessFailedCount;
            stringDb.LockoutEnabled = command.LockoutEnabled;
            stringDb.LockoutEnd = command.LockoutEnd;
            stringDb.Name = command.Name;
            stringDb.TwoFactorEnabled = command.TwoFactorEnabled;
            stringDb.PhoneNumber = command.PhoneNumber;
            stringDb.PhoneNumberConfirmed = command.PhoneNumberConfirmed;
            stringDb.SocialNumber = command.SocialNumber;
            stringDb.Birthdate = command.Birthdate;
        }

        public void UpdateProfile(UpdateProfileCommand command, CustomStringIdentity @string)
        {
            @string.Name = command.Name;
            @string.PhoneNumber = command.PhoneNumber;
            @string.Bio = command.Bio;
            @string.Company = command.Company;
            @string.JobTitle = command.JobTitle;
            @string.Url = command.Url;
            @string.SocialNumber = command.SocialNumber;
            @string.Birthdate = command.Birthdate;
        }
    }
}