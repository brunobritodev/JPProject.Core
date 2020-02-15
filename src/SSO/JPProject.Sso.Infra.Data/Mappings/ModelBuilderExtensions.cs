using JPProject.EntityFrameworkCore.Mappings;
using Microsoft.EntityFrameworkCore;

namespace JPProject.Sso.Infra.Data.Mappings
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureEventStoreContext(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new StoredEventMap());
            builder.ApplyConfiguration(new EventDetailsMap());
        }

        public static void ConfigureSsoContext(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new EmailMap());
            builder.ApplyConfiguration(new TemplateMap());
        }
    }
}
