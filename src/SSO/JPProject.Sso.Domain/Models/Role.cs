namespace JPProject.Sso.Domain.Models
{
    public class Role
    {
        public Role(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
    }
}
