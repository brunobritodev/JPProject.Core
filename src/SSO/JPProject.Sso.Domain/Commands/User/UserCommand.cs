using JPProject.Domain.Core.Commands;
using System;

namespace JPProject.Sso.Domain.Commands.User
{
    public abstract class UserCommand : Command
    {
        public string Email { get; protected set; }
        public string PhoneNumber { get; protected set; }
        public string Name { get; protected set; }
        public string Username { get; protected set; }
        public string Password { get; protected set; }
        public string ConfirmPassword { get; protected set; }
        public string Picture { get; protected set; }
        public string Provider { get; protected set; }
        public string ProviderId { get; protected set; }
        public bool EmailConfirmed { get; protected set; }
        public string Code { get; protected set; }
        public string EmailOrUsername { get; protected set; }
        public DateTime? Birthdate { get; internal set; }
        public string SocialNumber { get; internal set; }
        public bool CheckProvider { get; protected set; }
    }
}
