using JPProject.Sso.Domain.Interfaces;

namespace JPProject.Sso.Domain.Models
{
    public class Role : IRole
    {
        public Role(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}
