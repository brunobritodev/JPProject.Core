namespace JPProject.Sso.Application.ViewModels
{
    public class ConfigurationViewModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsPublic { get; set; }
        public bool IsSensitve { get; set; }
    }
}