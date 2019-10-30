namespace JPProject.EntityFrameworkCore.Configuration
{
    public class EventStoreMigrationOptions
    {
        public bool Migrate { get; set; }
        public static EventStoreMigrationOptions Get() { return new EventStoreMigrationOptions(); }
        public EventStoreMigrationOptions ShouldMigrate(bool migrate = true)
        {
            Migrate = migrate;
            return this;
        }
    }
}
