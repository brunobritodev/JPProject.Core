using JPProject.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;

namespace JPProject.Sso.Infra.CrossCutting.Identity.Models.Identity
{
    public class UserIdentity : IdentityUser<Guid>, IDomainUser
    {
        public string Picture { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Bio { get; set; }
        public string JobTitle { get; set; }
    }
}