using IdentityServer4.EntityFramework.Interfaces;
using JPProject.Admin.Infra.Data.Context;
using JPProject.Domain.Core.Interfaces;
using JPProject.EntityFrameworkCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace JPProject.Admin.Infra.Data.Repository
{
    /// <summary>
    /// IdentityServer4 does not have a implementation of DbSet, so isn't possible to construct Generic repo based in IIConfigurationDbContext and IPersistedGrantDbContext.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IConfigurationStore : IJpEntityFrameworkStore, IConfigurationDbContext
    {

    }
    public class RepositoryDemo<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IJpEntityFrameworkStore Db;
        protected readonly DbSet<TEntity> DbSet;

        public RepositoryDemo(IJpEntityFrameworkStore adminUiContext)
        {
            Db = adminUiContext;
            DbSet = Db.Set<TEntity>();
        }

        public virtual void Add(TEntity obj)
        {
            DbSet.Add(obj);
        }

        public virtual TEntity GetById<T>(T id)
        {
            return DbSet.Find(id);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public virtual void Update(TEntity obj)
        {
            DbSet.Update(obj);
        }

        public virtual void Remove<T>(T id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public int SaveChanges()
        {
            return Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
