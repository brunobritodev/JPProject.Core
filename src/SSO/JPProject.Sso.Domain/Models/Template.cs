using JPProject.Domain.Core.Models;
using System;

namespace JPProject.Sso.Domain.Models
{
    public class Template : Entity
    {
        // EF Constructor
        public Template() { }
        public Template(string content, string subject, string name, string username)
        {
            Id = Guid.NewGuid();
            Content = content;
            Subject = subject;
            Name = name;
            Username = username;
        }

        public string Subject { get; set; }
        public string Content { get; private set; }
        public string Name { get; private set; }

        public string Username { get; private set; }
        public DateTime Created { get; private set; } = DateTime.UtcNow;
        public DateTime Updated { get; private set; } = DateTime.UtcNow;

        public void UpdateTemplate(string content, string subject, string name, string username)
        {
            Subject = subject;
            Content = content;
            Name = name;
            Username = username;
            Updated = DateTime.UtcNow;
        }

    }
}
