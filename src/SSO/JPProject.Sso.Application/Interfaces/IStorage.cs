using JPProject.Sso.Application.ViewModels;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.Interfaces
{
    public interface IStorage
    {
        Task<string> Upload(FileUploadViewModel image);
        Task Remove(string filename, string virtualLocation);
    }

}
