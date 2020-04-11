using System;

namespace JPProject.Sso.Domain.ViewModels.Settings
{
    public class PublicSettings
    {
        public PublicSettings(Settings settings)
        {
            Logo = settings["Logo"];
            Version = new Version(settings["SSO:Version"]);
        }
        public Version Version { get; set; }
        public string Logo { get; set; }
    }
}