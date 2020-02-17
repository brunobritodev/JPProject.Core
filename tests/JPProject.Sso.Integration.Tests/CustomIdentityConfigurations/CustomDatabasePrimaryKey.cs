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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JPProject.Sso.Integration.Tests.CustomIdentityConfigurations
{

    public class CustomDatabasePrimaryKey<TUser, TRole, TKey> : IdentityDbContext<TUser, TRole, TKey>,
        IPersistedGrantDbContext,
        IConfigurationDbContext,
        ISsoContext,
        IEventStoreContext
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
    {
        private readonly ConfigurationStoreOptions _storeOptions;
        private readonly OperationalStoreOptions _operationalOptions;

        public CustomDatabasePrimaryKey(
            DbContextOptions<CustomDatabasePrimaryKey<TUser, TRole, TKey>> options,
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
            builder.Entity<TRole>().ToTable(TableConsts.IdentityRoles);
            builder.Entity<IdentityRoleClaim<TKey>>().ToTable(TableConsts.IdentityRoleClaims);
            builder.Entity<IdentityUserRole<TKey>>().ToTable(TableConsts.IdentityUserRoles);

            builder.Entity<TUser>().ToTable(TableConsts.IdentityUsers);
            builder.Entity<IdentityUserLogin<TKey>>().ToTable(TableConsts.IdentityUserLogins);
            builder.Entity<IdentityUserClaim<TKey>>().ToTable(TableConsts.IdentityUserClaims);
            builder.Entity<IdentityUserToken<TKey>>().ToTable(TableConsts.IdentityUserTokens);

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
