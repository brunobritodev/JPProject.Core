using IdentityServer4.Models;
using JPProject.Domain.Core.Interfaces;
using System.Threading.Tasks;

namespace JPProject.Admin.Domain.Interfaces
{
    public interface IApiScopeRepository : IRepository<ApiScope>
    {
        void RemoveScope(string name);
        Task<ApiScope> Get(string scopeName);
        Task UpdateWithChildren(string oldName, ApiScope scope);
    }
}