using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using JPProject.Domain.Core.Events;
using JPProject.EntityFrameworkCore.Interfaces;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Infra.Data.Constants;
using JPProject.Sso.Infra.Data.Interfaces;
using JPProject.Sso.Infra.Data.Mappings;
using JPProject.Sso.Infra.Identity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JPProject.Sso.Integration.Tests.Context
{
    public class UnifiedContext : IdentityDbContext<UserIdentity>,
       IPersistedGrantDbContext,
       IConfigurationDbContext,
       ISsoContext,
       IEventStoreContext
    {
        private readonly ConfigurationStoreOptions _storeOptions;
        private readonly OperationalStoreOptions _operationalOptions;

        public UnifiedContext(
            DbContextOptions<UnifiedContext> options,
            ConfigurationStoreOptions storeOptions,
            OperationalStoreOptions operationalOptions)
            : base(options)
        {
            _storeOptions = storeOptions;
            _operationalOptions = operationalOptions;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ConfigureIdentityContext(builder);
        }

        private void ConfigureIdentityContext(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().ToTable(TableConsts.IdentityRoles);
            builder.Entity<IdentityRoleClaim<string>>().ToTable(TableConsts.IdentityRoleClaims);
            builder.Entity<IdentityUserRole<string>>().ToTable(TableConsts.IdentityUserRoles);

            builder.Entity<UserIdentity>().ToTable(TableConsts.IdentityUsers);
            builder.Entity<IdentityUserLogin<string>>().ToTable(TableConsts.IdentityUserLogins);
            builder.Entity<IdentityUserClaim<string>>().ToTable(TableConsts.IdentityUserClaims);
            builder.Entity<IdentityUserToken<string>>().ToTable(TableConsts.IdentityUserTokens);

            builder.ConfigureClientContext(_storeOptions);
            builder.ConfigureResourcesContext(_storeOptions);
            builder.ConfigurePersistedGrantContext(_operationalOptions);
            builder.ConfigureEventStoreContext();
            builder.ConfigureSsoContext();
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        public DbSet<Template> Templates { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<GlobalConfigurationSettings> GlobalConfigurationSettings { get; set; }

        public DbSet<StoredEvent> StoredEvent { get; set; }
        public DbSet<EventDetails> StoredEventDetails { get; set; }

    }
}
