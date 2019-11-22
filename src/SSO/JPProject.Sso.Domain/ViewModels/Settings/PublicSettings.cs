namespace JPProject.Sso.Domain.ViewModels.Settings
{
    public class PublicSettings
    {
        public PublicSettings(Settings settings)
        {
            Logo = settings["Logo"];
        }

        public string Logo { get; set; }
    }
}