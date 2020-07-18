using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Domain.Core.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JPProject.Admin.EntityFramework.Repository.Repository
{
    public class ApiScopeRepository : IApiScopeRepository
    {
        public DbSet<ApiScope> DbSet { get; set; }
        public ApiScopeRepository(IConfigurationDbContext context)
        {
            DbSet = context.ApiScopes;
        }


        public void Add(IdentityServer4.Models.ApiScope obj)
        {
            DbSet.Add(obj.ToEntity());
        }

        public void Update(IdentityServer4.Models.ApiScope obj)
        {
            var apiResource = DbSet.FirstOrDefault(w => w.Name == obj.Name);
            var newOne = obj.ToEntity();
            newOne.Id = apiResource.Id;
            DbSet.Update(newOne);
        }

        public void Remove(IdentityServer4.Models.ApiScope obj)
        {
            var apiResource = DbSet.FirstOrDefault(w => w.Name == obj.Name);
            DbSet.Remove(apiResource);
        }

        public void RemoveScope(string name)
        {
            var apiResource = DbSet.FirstOrDefault(w => w.Name == name);
            DbSet.Remove(apiResource);
        }

        public async Task<IdentityServer4.Models.ApiScope> Get(string scopeName)
        {
            var scope = await DbSet
                                    .Include(s => s.Properties)
                                    .Include(i => i.UserClaims)
                                    .FirstOrDefaultAsync(w => w.Name == scopeName);
            return scope.ToModel();
        }

        public async Task UpdateWithChildren(string oldName, IdentityServer4.Models.ApiScope scope)
        {
            var apiDb = await DbSet
                .Include(s => s.UserClaims)
                .Include(i => i.Properties)
                .Where(x => x.Name == oldName).FirstAsync();
            var newIr = scope.ToEntity();

            newIr.Id = apiDb.Id;
            newIr.ShallowCopyTo(apiDb);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
