using System.ComponentModel;

namespace JPProject.Sso.Domain.ViewModels.Settings
{
    public enum LoginStrategyType
    {
        [Description("ASP.NET Identity")]
        Identity = 1,
        [Description("LDAP")]
        Ldap = 2,

    }
}