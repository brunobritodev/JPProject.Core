using JPProject.Domain.Core.Commands;
using System;

namespace JPProject.Sso.Domain.Commands.UserManagement
{
    public abstract class ProfileCommand : Command
    {
        public string Username { get; protected set; }
        public string PhoneNumber { get; protected set; }
        public string Name { get; protected set; }
        public string Picture { get; protected set; }
        public string Url { get; protected set; }
        public string Company { get; protected set; }
        public string Bio { get; protected set; }
        public string JobTitle { get; protected set; }
        public string SocialNumber { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
