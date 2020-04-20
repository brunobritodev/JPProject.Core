using System;

namespace JPProject.Sso.Domain.ViewModels.Settings
{
    public class PublicSettings
    {
        public PublicSettings(Settings settings)
        {
            Logo = settings["Logo"];
            Version = new Version(settings["SSO:Version"]);
            UseRecaptcha = settings["UseRecaptcha"] == "true";
            RecaptchaSiteKey = settings["Recaptcha:SiteKey"];
        }

        public string RecaptchaSiteKey { get; set; }
        public bool UseRecaptcha { get; }
        public Version Version { get; }
        public string Logo { get; }
    }
}