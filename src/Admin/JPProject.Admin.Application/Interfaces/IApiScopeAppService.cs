using IdentityServer4.Models;
using System;
using System.Threading.Tasks;

namespace JPProject.Admin.Application.Interfaces
{
    public interface IApiScopeAppService : IDisposable
    {
        Task<bool> Remove(string name);
        Task<bool> Save(ApiScope model);
        Task<bool> Update(string oldName, ApiScope apiScope);
    }
}