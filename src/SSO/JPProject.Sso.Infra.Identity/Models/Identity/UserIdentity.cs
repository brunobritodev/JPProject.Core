using JPProject.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

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
    }
}