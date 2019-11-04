using JPProject.Domain.Core.Commands;

namespace JPProject.Sso.Domain.Commands.User
{
    public abstract class UserClaimCommand : Command
    {
        public int Id { get; protected set; }
        public string Username { get; protected set; }
        public string Type { get; protected set; }

        public string Value { get; protected set; }
    }
}