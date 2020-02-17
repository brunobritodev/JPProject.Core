using Microsoft.AspNetCore.Identity;
using System;

namespace JPProject.Sso.Integration.Tests.CustomIdentityConfigurations.GuidIdentity
{
    public class CustomRoleGuidIdentity : IdentityRole<Guid>
    {
        public CustomRoleGuidIdentity() { }
        public CustomRoleGuidIdentity(string roleName) : base(roleName)
        {
        }
    }
}