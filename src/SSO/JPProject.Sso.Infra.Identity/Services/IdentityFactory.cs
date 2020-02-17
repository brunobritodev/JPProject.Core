using JPProject.Sso.Domain.Commands.User;
using JPProject.Sso.Domain.Commands.UserManagement;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Identity.Models.Identity;

namespace JPProject.Sso.Infra.Identity.Services
{
    public class IdentityFactory : IIdentityFactory<UserIdentity>, IRoleFactory<RoleIdentity>
    {
        public UserIdentity Create(UserCommand user)
        {
            return new UserIdentity
            {
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

        public RoleIdentity CreateRole(string name)
        {
            return new RoleIdentity(name);
        }


        public void UpdateInfo(AdminUpdateUserCommand command, UserIdentity userDb)
        {
            userDb.Email = command.Email;
            userDb.EmailConfirmed = command.EmailConfirmed;
            userDb.AccessFailedCount = command.AccessFailedCount;
            userDb.LockoutEnabled = command.LockoutEnabled;
            userDb.LockoutEnd = command.LockoutEnd;
            userDb.Name = command.Name;
            userDb.TwoFactorEnabled = command.TwoFactorEnabled;
            userDb.PhoneNumber = command.PhoneNumber;
            userDb.PhoneNumberConfirmed = command.PhoneNumberConfirmed;
            userDb.SocialNumber = command.SocialNumber;
            userDb.Birthdate = command.Birthdate;
        }

        public void UpdateProfile(UpdateProfileCommand command, UserIdentity user)
        {
            user.Name = command.Name;
            user.PhoneNumber = command.PhoneNumber;
            user.Bio = command.Bio;
            user.Company = command.Company;
            user.JobTitle = command.JobTitle;
            user.Url = command.Url;
            user.SocialNumber = command.SocialNumber;
            user.Birthdate = command.Birthdate;
        }

    }
}
