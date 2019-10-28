using JPProject.Domain.Core.Commands;

namespace JPProject.Sso.Domain.Commands.User
{
    public abstract class UserRoleCommand : Command
    {
        public string Username { get; protected set; }
        public string Role { get; protected set; }
    }
}