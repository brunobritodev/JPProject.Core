using JPProject.Sso.Infra.Data.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace JPProject.Sso.Infra.Data.Context
{

    /// <summary>
    /// Context for the Entity Framework database context used for Sso.
    /// </summary>
    /// <typeparam name="TUser">The type of user objects.</typeparam>
    /// <typeparam name="TRole">The type of role objects.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
    /// <typeparam name="TUserClaim">The type of the user claim object.</typeparam>
    /// <typeparam name="TUserRole">The type of the user role object.</typeparam>
    /// <typeparam name="TUserLogin">The type of the user login object.</typeparam>
    /// <typeparam name="TRoleClaim">The type of the role claim object.</typeparam>
    /// <typeparam name="TUserToken">The type of the user token object.</typeparam>
    public abstract class ApplicationIdentityContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
      where TUser : IdentityUser<TKey>
      where TRole : IdentityRole<TKey>
      where TKey : IEquatable<TKey>
      where TUserClaim : IdentityUserClaim<TKey>
      where TUserRole : IdentityUserRole<TKey>
      where TUserLogin : IdentityUserLogin<TKey>
      where TRoleClaim : IdentityRoleClaim<TKey>
      where TUserToken : IdentityUserToken<TKey>
    {
        /// <summary>Initializes a new instance of the class.</summary>
        /// <param name="options">The options to be used by a <see cref="T:Microsoft.EntityFrameworkCore.DbContext" />.</param>
        protected ApplicationIdentityContext(DbContextOptions options)
          : base(options)
        {
        }

        /// <summary>
        /// Configures the schema needed for the identity framework.
        /// </summary>
        /// <param name="builder">
        /// The builder being used to construct the model for this context.
        /// </param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole<TKey>>().ToTable(TableConsts.IdentityRoles);
            builder.Entity<IdentityRoleClaim<TKey>>().ToTable(TableConsts.IdentityRoleClaims);
            builder.Entity<IdentityUserRole<TKey>>().ToTable(TableConsts.IdentityUserRoles);

            builder.Entity<IdentityUser<TKey>>().ToTable(TableConsts.IdentityUsers);
            builder.Entity<IdentityUserLogin<TKey>>().ToTable(TableConsts.IdentityUserLogins);
            builder.Entity<IdentityUserClaim<TKey>>().ToTable(TableConsts.IdentityUserClaims);
            builder.Entity<IdentityUserToken<TKey>>().ToTable(TableConsts.IdentityUserTokens);
        }
    }

    /// <summary>
    /// Sso base context
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class ApplicationIdentityContext<TUser, TRole, TKey> : ApplicationIdentityContext<TUser, TRole, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>

    {
        public ApplicationIdentityContext(
            DbContextOptions<ApplicationIdentityContext<TUser, TRole, TKey>> options)
            : base(options)
        {
        }
    }

    /// <summary>
    /// SSO base class.
    /// </summary>
    public class ApplicationIdentityContext : ApplicationIdentityContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        public ApplicationIdentityContext(DbContextOptions<ApplicationIdentityContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>> options)
            : base(options)
        {
        }
    }

    /// <summary>
    /// SSO base class.
    /// </summary>
    public class ApplicationIdentityContext<TKey> : ApplicationIdentityContext<IdentityUser<TKey>, IdentityRole<TKey>, TKey>
        where TKey : IEquatable<TKey>
    {
        public ApplicationIdentityContext(DbContextOptions<ApplicationIdentityContext<IdentityUser<TKey>, IdentityRole<TKey>, TKey>> options)
         : base(options)
        {
        }
    }

}
