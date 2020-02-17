using JPProject.Domain.Core.Interfaces;
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
        public void UpdatePicture(string picture)
        {
            this.Picture = picture;
        }

        public void ConfirmEmail()
        {
            EmailConfirmed = true;
        }
    }

    public class RoleIdentity : IdentityRole
    {
        public RoleIdentity() : base() { }
        public RoleIdentity(string name) : base(name) { }
    }
}