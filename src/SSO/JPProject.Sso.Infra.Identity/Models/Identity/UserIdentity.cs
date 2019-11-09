using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace JPProject.Sso.Infra.Identity.Models.Identity
{
    public class UserIdentity : IdentityUser, IDomainUser
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
    }
}