using System.Linq;

namespace JPProject.Sso.Domain.ViewModels.Settings
{
    public class PrivateSettings
    {
        public PrivateSettings(Settings settings)
        {
            if (!settings.Any())
                return;

            Smtp = new Smtp(settings["Smtp:Server"], settings["Smtp:Port"], settings["Smtp:UseSsl"], settings["Smtp:Password"], settings["Smtp:Username"]);
            Storage = new StorageSettings(
                settings["Repository:Username"],
                settings["Repository:Password"],
                settings["Repository:Service"],
                settings["Repository:StorageName"],
                settings["Repository:PhysicalPath"],
                settings["Repository:VirtualPath"],
                settings["Repository:BasePath"],
                        settings["Repository:Region"]);

            if (bool.TryParse(settings["SendEmail"], out _))
                SendEmail = bool.Parse(settings["SendEmail"]);

            if (bool.TryParse(settings["UseStorage"], out _))
                UseStorage = bool.Parse(settings["UseStorage"]);


        }

        public bool UseStorage { get; }
        public bool SendEmail { get; }
        public Smtp Smtp { get; }
        public StorageSettings Storage { get; }
    }
}