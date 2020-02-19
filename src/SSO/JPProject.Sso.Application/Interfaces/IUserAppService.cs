using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Application.ViewModels.UserViewModels;
using System;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.Interfaces
{
    public interface IUserAppService : IDisposable
    {
        Task<bool> AdminRegister(AdminRegisterUserViewModel model);
        Task<bool> Register(RegisterUserViewModel model);
        Task<bool> RegisterWithoutPassword(SocialViewModel model);
        Task<bool> CheckUsername(string userName);
        Task<bool> CheckEmail(string email);
        Task<bool> RegisterWithProvider(RegisterUserViewModel model);
        Task<bool> SendResetLink(ForgotPasswordViewModel model);
        Task<bool> ResetPassword(ResetPasswordViewModel model);
        Task<bool> ConfirmEmail(ConfirmEmailViewModel model);
        Task<bool> AddLogin(SocialViewModel user);
    }
}
