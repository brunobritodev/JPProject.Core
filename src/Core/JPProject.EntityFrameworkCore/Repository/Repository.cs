using JPProject.Domain.Core.Interfaces;
using JPProject.EntityFrameworkCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace JPProject.EntityFrameworkCore.Repository
{
    /// <summary>
    /// Generic repository for <see cref="T:JPProject.EntityFrameworkCore.Interfaces.IJpEntityFrameworkStore" />
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class Repository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : IJpEntityFrameworkStore
    {
        protected readonly TContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(TContext context)
        {
            Db = context;
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
