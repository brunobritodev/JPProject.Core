using JPProject.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;

namespace JPProject.Sso.Integration.Tests.CustomIdentityConfigurations.StringIdentity
{
    public class CustomStringIdentity : IdentityUser<string>, IDomainUser
    {
        public string Picture { get; internal set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Company { get; set; }
        public string Bio { get; set; }
        public string JobTitle { get; set; }
        public string SocialNumber { get; set; }
        public DateTime? Birthdate { get; set; }
        public string MyCustomField { get; set; }
        public void UpdatePicture(string picture)
        {
            Picture = picture;
        }

        public void ConfirmEmail()
        {
            EmailConfirmed = true;
        }
    }
}
