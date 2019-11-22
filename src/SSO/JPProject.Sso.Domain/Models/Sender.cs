namespace JPProject.Sso.Domain.Models
{
    public class Sender
    {
        public Sender() { }
        public Sender(string address, string name)
        {
            Address = address;
            Name = name;
        }

        public string Address { get; }
        public string Name { get; }
    }
}