using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.ViewModels;
using JPProject.Sso.Domain.Commands.Role;
using JPProject.Sso.Domain.Commands.User;
using JPProject.Sso.Domain.Commands.UserManagement;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Domain.ViewModels.User;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JPProject.Sso.Domain.Interfaces
{
    public interface IUserService
    {
        Task<AccountResult?> CreateUserWithPass(IDomainUser user, string password);
        Task<AccountResult?> CreateUserWithProvider(IDomainUser user, string provider, string providerUserId);
        Task<AccountResult?> CreateUserWithProviderAndPass(IDomainUser user, string password, string provider, string providerId);
        Task<bool> UsernameExist(string userName);
        Task<bool> EmailExist(string email);
        Task<AccountResult?> GenerateResetPasswordLink(string emailOrUsername);
        Task<string> ResetPassword(ResetPasswordCommand request);
        Task<string> ConfirmEmailAsync(ConfirmEmailCommand command);
        Task<bool> UpdateProfileAsync(UpdateProfileCommand command);
        Task<bool> UpdateProfilePictureAsync(UpdateProfilePictureCommand command);
        Task<bool> CreatePasswordAsync(SetPasswordCommand request);
        Task<bool> ChangePasswordAsync(ChangePasswordCommand request);
        Task<bool> RemoveAccountAsync(RemoveAccountCommand request);
        Task<bool> HasPassword(string userId);
        Task<IEnumerable<User>> GetUsers(PagingViewModel page);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByNameAsync(string username);
        Task<User> FindByProviderAsync(string provider, string providerUserId);
        Task<User> FindByUserId(string userId);
        Task<IEnumerable<User>> GetByIdAsync(params string[] id);
        Task<User> FindByUsernameOrEmail(string emailOrUsername);
        Task UpdateUserAsync(User user);
        Task<IEnumerable<Claim>> GetClaimByName(string userName);
        Task<bool> SaveClaim(string userId, Claim claim);
        Task<bool> RemoveClaim(RemoveUserClaimCommand command);
        Task<IEnumerable<string>> GetRoles(string userName);
        Task<bool> RemoveRole(RemoveUserRoleCommand command);
        Task<bool> SaveRole(SaveUserRoleCommand command);
        Task<IEnumerable<UserLogin>> GetUserLogins(string userName);
        Task<bool> RemoveLogin(RemoveUserLoginCommand command);
        Task<IEnumerable<User>> GetUserFromRole(string role);
        Task<bool> RemoveUserFromRole(RemoveUserFromRoleCommand command);
        Task<bool> ResetPasswordAsync(AdminChangePasswordCommand command);
        Task<string> AddLoginAsync(AddLoginCommand command);
        Task<int> Count(string search);
    }
}