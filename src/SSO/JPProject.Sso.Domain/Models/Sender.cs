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

        public string Address { get; set; }
        public string Name { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) || !string.IsNullOrEmpty(Address);
        }
    }
}