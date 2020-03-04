using System;
using Microsoft.AspNetCore.Identity;

namespace JPProject.Sso.Integration.Tests.CustomIdentityConfigurations.StringIdentity
{
    public class CustomRoleStringIdentity : IdentityRole<string>
    {
        public CustomRoleStringIdentity() { }
        public CustomRoleStringIdentity(string roleName) : base(roleName)
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}