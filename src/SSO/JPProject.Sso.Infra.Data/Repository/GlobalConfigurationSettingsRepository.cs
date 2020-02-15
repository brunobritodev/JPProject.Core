using System.Linq;
using JPProject.EntityFrameworkCore.Repository;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JPProject.Sso.Infra.Data.Repository
{
    public class GlobalConfigurationSettingsRepository : Repository<GlobalConfigurationSettings>, IGlobalConfigurationSettingsRepository
    {
        public GlobalConfigurationSettingsRepository(ISsoContext context) : base(context)
        {
        }

        public Task<GlobalConfigurationSettings> FindByKey(string key)
        {
            return DbSet.FirstOrDefaultAsync(f => f.Key == key);
        }

        public IQueryable<GlobalConfigurationSettings> GetAll()
        {
            return DbSet.AsQueryable();
        }
    }
}
