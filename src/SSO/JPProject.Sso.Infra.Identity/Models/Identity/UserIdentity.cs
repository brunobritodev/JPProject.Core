using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Domain.Commands.UserManagement;
using JPProject.Sso.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace JPProject.Sso.Infra.Identity.Models.Identity
{
    public class UserIdentity : IdentityUser, IDomainUser, IDomainUserFactory<UserIdentity>
    {
        public string Picture { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Bio { get; set; }
        public string JobTitle { get; set; }
        /// <summary>
        /// Country unique number
        /// e.g:
        /// Social Security Number (USA)
        /// RG or Cpf (Brazil)
        /// Burgerservicenumber (Netherlands)
        /// Henkilötunnus (Finnish)
        /// NIF (Portugal)
        /// </summary>
        public string SocialNumber { get; set; }
        public DateTime? Birthdate { get; set; }

        public User ToUser()
        {
            return new User(
                Id,
                Email,
                EmailConfirmed,
                Name,
                SecurityStamp,
                AccessFailedCount,
                Bio,
                Company,
                JobTitle,
                LockoutEnabled,
                LockoutEnd,
                PhoneNumber,
                PhoneNumberConfirmed,
                Picture,
                TwoFactorEnabled,
                Url,
                UserName,
                Birthdate,
                SocialNumber
            );
        }
        public void UpdateInfo(UpdateUserCommand user)
        {
            Email = user.Email;
            EmailConfirmed = user.EmailConfirmed;
            AccessFailedCount = user.AccessFailedCount;
            LockoutEnabled = user.LockoutEnabled;
            LockoutEnd = user.LockoutEnd;
            Name = user.Name;
            TwoFactorEnabled = user.TwoFactorEnabled;
            PhoneNumber = user.PhoneNumber;
            PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            SocialNumber = user.SocialNumber;
            Birthdate = user.Birthdate;
        }

        public void UpdateBio(UpdateProfileCommand command)
        {
            Name = command.Name;
            Bio = command.Bio;
            Company = command.Company;
            JobTitle = command.JobTitle;
            Url = command.Url;
            PhoneNumber = command.PhoneNumber;
            SocialNumber = command.SocialNumber;
            Birthdate = command.Birthdate;
        }

        public UserIdentity CreateUser(IDomainUser user)
        {
            return new UserIdentity
            {
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserName = user.UserName,
                Name = user.Name,
                Picture = user.Picture,
                EmailConfirmed = user.EmailConfirmed,
                SocialNumber = user.SocialNumber,
                Birthdate = user.Birthdate,
                Bio = user.Bio,
                JobTitle = user.JobTitle,
                LockoutEnd = null,
                Company = user.Company,
            };
        }
    }
}