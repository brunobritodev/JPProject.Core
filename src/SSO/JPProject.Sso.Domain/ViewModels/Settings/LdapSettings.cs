namespace JPProject.Sso.Domain.ViewModels.Settings
{
    public class LdapSettings
    {
        public LdapSettings() { }
        public LdapSettings(string domainName, string distinguishedName, string attributes, string authType, string searchScope, string portNumber, string fullyQualifiedDomainName, string connectionLess, string address)
        {
            DomainName = domainName;
            DistinguishedName = distinguishedName;
            Attributes = attributes;
            AuthType = authType;
            SearchScope = searchScope;
            Address = address;
            bool.TryParse(connectionLess, out var connectionParse);
            ConnectionLess = connectionParse;
            int.TryParse(portNumber, out var number);
            PortNumber = number;

            bool.TryParse(fullyQualifiedDomainName, out var fqdn);
            FullyQualifiedDomainName = fqdn;
        }
        public string DomainName { get; set; }
        public string DistinguishedName { get; set; }
        public string Attributes { get; set; }
        public string AuthType { get; set; }
        public string SearchScope { get; set; }
        public string Address { get; }
        public bool FullyQualifiedDomainName { get; }
        public bool ConnectionLess { get; }
        public int PortNumber { get; set; }
    }
}