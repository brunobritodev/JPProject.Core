using System.Threading.Tasks;
using JPProject.Sso.Application.ViewModels;

namespace JPProject.Sso.Application.Interfaces
{
    public interface IImageStorage
    {
        Task<string> SaveAsync(ProfilePictureViewModel image);
    }

}
