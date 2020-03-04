using Microsoft.AspNetCore.Identity;

namespace JPProject.Sso.Integration.Tests.CustomIdentityConfigurations.IntIdentity
{
    public class CustomRoleIntIdentity : IdentityRole<int>
    {
        public CustomRoleIntIdentity() { }
        public CustomRoleIntIdentity(string roleName) : base(roleName)
        {
        }
    }
}