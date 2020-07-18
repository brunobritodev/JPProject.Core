using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Interfaces;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Domain.Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace JPProject.Admin.EntityFramework.Repository.Repository
{
    public class ResourceRepository : IResourceRepository
    {
        protected readonly IConfigurationDbContext Context;

        public ResourceRepository(IConfigurationDbContext context)
        {
            this.Context = context;
        }

        public async Task<ResourceList> Search(string search)
        {
            var identity = Context.IdentityResources.AsNoTracking().Where(w => w.Name.Contains(search)).Select(s => s.Name).ToArrayAsync();

            var apis = Context.ApiResources.AsNoTracking().Where(w => w.Name.Contains(search)).Select(s => s.Name).ToArrayAsync();

            var scopes = Context.ApiScopes.AsNoTracking().Where(w => w.Name.Contains(search)).Select(s => s.Name).ToArrayAsync();

            var allResources = await Task.WhenAll(identity, apis, scopes);

            return new ResourceList(allResources.SelectMany(s => s));
        }

        public async Task<ResourceList> All()
        {
            var identity = Context.IdentityResources.AsNoTracking().Select(s => s.Name).ToArrayAsync();

            var apis = Context.ApiResources.AsNoTracking().Select(s => s.Name).ToArrayAsync();

            var scopes = Context.ApiScopes.AsNoTracking().Select(s => s.Name).ToArrayAsync();

            var allResources = await Task.WhenAll(identity, apis, scopes);

            return new ResourceList(allResources.SelectMany(s => s));
        }
    }
}