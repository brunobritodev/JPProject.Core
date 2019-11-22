namespace JPProject.Sso.Domain.Models
{
    public class Role
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
