using System;

namespace JPProject.Sso.Domain.Models
{
    public class Role
    {
        public Role(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
    }
}
