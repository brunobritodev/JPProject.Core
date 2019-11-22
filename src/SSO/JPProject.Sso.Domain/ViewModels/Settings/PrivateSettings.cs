using Microsoft.EntityFrameworkCore.Internal;

namespace JPProject.Sso.Domain.ViewModels.Settings
{
    public class PrivateSettings
    {
        public PrivateSettings(Settings settings)
        {
            if (!settings.Any())
                return;

            Smtp = new Smtp(settings["Smtp:Server"], settings["Smtp:Port"], settings["Smtp:UseSsl"], settings["Smtp:Password"], settings["Smtp:Username"]);

            if (bool.TryParse(settings["SendEmail"], out _))
                SendEmail = bool.Parse(settings["SendEmail"]);
        }

        public bool SendEmail { get; set; }
        public Smtp Smtp { get; set; }
    }
}