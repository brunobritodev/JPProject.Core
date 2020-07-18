using JPProject.Domain.Core.Commands;

namespace JPProject.Admin.Domain.Commands.ApiScope
{
    public abstract class ApiScopeCommand : Command
    {
        public string OldName { get; protected set; }

        public IdentityServer4.Models.ApiScope ApiScope { get; protected set; }
        public string ResourceName { get; protected set; }
    }
}