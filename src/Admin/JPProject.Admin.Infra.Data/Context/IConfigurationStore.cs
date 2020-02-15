using IdentityServer4.EntityFramework.Interfaces;
using JPProject.EntityFrameworkCore.Interfaces;

namespace JPProject.Admin.Infra.Data.Context
{
    /// <summary>
    /// IdentityServer4 does not have a implementation of DbSet, so isn't possible to construct Generic repo based in IIConfigurationDbContext and IPersistedGrantDbContext.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IConfigurationStore : IJpEntityFrameworkStore, IConfigurationDbContext
    {

    }
}   
