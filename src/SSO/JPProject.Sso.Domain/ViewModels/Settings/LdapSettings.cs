namespace JPProject.Sso.Domain.ViewModels.Settings
{
    public class LdapSettings
    {
        public LdapSettings() { }
        public LdapSettings(string domainName, string distinguishedName, string attributes, string authType, string searchScope)
        {
            DomainName = domainName;
            DistinguishedName = distinguishedName;
            Attributes = attributes;
            AuthType = authType;
            SearchScope = searchScope;
        }
        public string DomainName { get; set; }
        public string DistinguishedName { get; set; }
        public string Attributes { get; set; }
        public string AuthType { get; set; }
        public string SearchScope { get; set; }
    }
}