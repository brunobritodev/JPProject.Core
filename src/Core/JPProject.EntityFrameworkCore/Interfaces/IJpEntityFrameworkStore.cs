using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JPProject.EntityFrameworkCore.Interfaces
{
    public interface IJpEntityFrameworkStore : IDisposable
    {
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync();
    }
}
