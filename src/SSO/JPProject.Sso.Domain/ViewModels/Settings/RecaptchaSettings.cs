namespace JPProject.Sso.Domain.ViewModels.Settings
{
    public class RecaptchaSettings
    {
        public string SiteKey { get; }
        public string PrivateKey { get; }
        public RecaptchaSettings(string siteKey, string privateKey)
        {
            SiteKey = siteKey;
            PrivateKey = privateKey;
        }
    }
}