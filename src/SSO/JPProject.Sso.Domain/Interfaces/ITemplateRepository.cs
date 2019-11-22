using System.Collections.Generic;
using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Domain.Models;
using System.Threading.Tasks;

namespace JPProject.Sso.Domain.Interfaces
{
    public interface ITemplateRepository : IRepository<Template>
    {
        Task<bool> Exist(string name);
        Task<Template> GetByName(string name);
        Task<List<Template>> All();
    }
}
