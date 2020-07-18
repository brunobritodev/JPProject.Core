using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Domain.Core.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JPProject.Admin.EntityFramework.Repository.Repository.ModelMappers;

namespace JPProject.Admin.EntityFramework.Repository.Repository
{
    public class ApiResourceRepository : IApiResourceRepository
    {
        public DbSet<ApiResource> DbSet { get; set; }
        public ApiResourceRepository(IConfigurationDbContext context)
        {
            DbSet = context.ApiResources;
        }

        private IdentityServer4.Models.ApiResource ToModel(ApiResource arg)
        {
            return arg.ToModel();
        }

        public async Task<List<IdentityServer4.Models.ApiResource>> GetResources()
        {
            var apiResource = await DbSet.Include(s => s.UserClaims).ToListAsync();
            return apiResource.Select(ToModel).ToList();
        }


        public async Task<IdentityServer4.Models.ApiResource> GetResource(string name)
        {
            var apiResource = await DbSet
                .Include(s => s.Secrets)
                .Include(s => s.Scopes)
                .Include(s => s.Properties)
                .Include(s => s.UserClaims)
                                    .FirstOrDefaultAsync(s => s.Name == name);

            return apiResource.ToModel();
        }

        public void RemoveSecret(string resourceName, IdentityServer4.Models.Secret secret)
        {
            var client = GetResourceByName(resourceName);
            client.Secrets.RemoveAll(r => r.Type == secret.Type && r.Value == secret.Value);

            DbSet.Update(client);
        }

        public void AddSecret(string resourceName, IdentityServer4.Models.Secret secret)
        {
            var apiResource = GetResourceByName(resourceName);
            var entity = Mapper.Map<IdentityServer4.EntityFramework.Entities.ApiResourceSecret>(secret);
            apiResource.Secrets.Add(entity);
        }

        public void RemoveScope(string resourceName, string name)
        {
            var apiResource = GetResourceByName(resourceName);
            apiResource.Scopes.RemoveAll(r => r.Scope == name);
        }

        private ApiResource GetResourceByName(string resourceName)
        {
            var apiResource = DbSet
                                .Include(s => s.Secrets)
                                .Include(s => s.Scopes)
                                .Include(s => s.Properties)
                                .Include(s => s.UserClaims)
                                .FirstOrDefault(s => s.Name == resourceName);
            return apiResource;
        }

        public void AddScope(string resourceName, string scope)
        {
            var apiResource = GetResourceByName(resourceName);
            var entity = new ApiResourceScope() { ApiResourceId = apiResource.Id, Scope = scope };
            apiResource.Scopes.Add(entity);
        }

        public async Task<IEnumerable<IdentityServer4.Models.Secret>> GetSecretsByApiName(string name)
        {
            var secrets = await DbSet.Where(w => w.Name == name).SelectMany(s => s.Secrets).ToListAsync();
            return Mapper.Map<IEnumerable<IdentityServer4.Models.Secret>>(secrets);
        }


        public async Task UpdateWithChildrens(string oldName, IdentityServer4.Models.ApiResource irs)
        {
            var apiDb = await DbSet
                .Include(s => s.UserClaims)
                .Where(x => x.Name == oldName).FirstAsync();
            var newIr = irs.ToEntity();

            newIr.Id = apiDb.Id;
            newIr.ShallowCopyTo(apiDb);
        }

        public void Add(IdentityServer4.Models.ApiResource obj)
        {
            DbSet.Add(obj.ToEntity());
        }

        public void Update(IdentityServer4.Models.ApiResource obj)
        {
            var apiResource = DbSet.FirstOrDefault(w => w.Name == obj.Name);
            var newOne = obj.ToEntity();
            newOne.Id = apiResource.Id;
            DbSet.Update(newOne);
        }

        public void Remove(IdentityServer4.Models.ApiResource obj)
        {
            var apiResource = DbSet.FirstOrDefault(w => w.Name == obj.Name);
            DbSet.Remove(apiResource);
        }

        public async Task<IEnumerable<IdentityServer4.Models.Secret>> GetByApiName(string name)
        {
            var api = await GetResource(name);
            return api.ApiSecrets;
        }

        public async Task<List<string>> SearchScopes(string search)
        {
            var scopes = await DbSet.Where(id => id.Name.Contains(search)).SelectMany(s => s.Scopes).ToListAsync();
            return scopes.Select(s => s.Scope).ToList();
        }

        public async Task<IEnumerable<string>> GetScopesByResource(string search)
        {
            var scopes = await DbSet.Include(s => s.Scopes).Where(id => id.Name == search).SelectMany(s => s.Scopes).ToListAsync();

            return scopes.Select(s => s.Scope);
        }

        public Task<List<string>> ListResources()
        {
            return DbSet.Select(s => s.Name).ToListAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}