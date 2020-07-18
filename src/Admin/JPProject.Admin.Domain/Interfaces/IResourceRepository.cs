using JPProject.Domain.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JPProject.Admin.Domain.Interfaces
{
    public interface IResourceRepository
    {
        Task<ResourceList> Search(string search);
        Task<ResourceList> All();
    }
}