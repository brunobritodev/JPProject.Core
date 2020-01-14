using System.Threading.Tasks;
using JPProject.Sso.Application.ViewModels;

namespace JPProject.Sso.Application.CloudServices.Storage
{
    public interface IStorageService
    {
        Task<string> Upload(FileUploadViewModel file);
        Task RemoveFile(string fileName, string virtualLocation);
    }
}