using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Application.ViewModels.UserViewModels;
using System;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.Interfaces
{
    public interface IUserAppService : IDisposable
    {
        /// <summary>
        /// Register user as an admin. Bypass many validation rules
        /// </summary>
        Task<bool> AdminRegister(AdminRegisterUserViewModel model);

        /// <summary>
        /// Register regular user. With password and without Provider
        /// </summary>
        Task<bool> Register(RegisterUserViewModel model);

        /// <summary>
        /// Register user from LDAP connection
        /// </summary>
        Task<bool> Register(RegisterUserLdapViewModel model);

        /// <summary>
        /// Register user and add a new Login for him. Usually for federation logins
        /// </summary>
        Task<bool> RegisterWithoutPassword(SocialViewModel model);

        Task<bool> CheckUsername(string userName);
        Task<bool> CheckEmail(string email);

        /// <summary>
        /// Register user with password and add a new Login for him.
        /// </summary>
        Task<bool> RegisterWithPasswordAndProvider(RegisterUserViewModel model);
        Task<bool> SendResetLink(ForgotPasswordViewModel model);
        Task<bool> ResetPassword(ResetPasswordViewModel model);
        Task<bool> ConfirmEmail(ConfirmEmailViewModel model);
        Task<bool> AddLogin(SocialViewModel user);
    }
}
