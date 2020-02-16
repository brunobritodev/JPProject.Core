using AspNetCore.IQueryable.Extensions;
using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Domain.Commands.UserManagement;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Domain.ViewModels.User;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using JPProject.Domain.Core.ViewModels;

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
        Task<string> ConfirmEmailAsync(string email, string code);
        Task<bool> UpdateProfileAsync(UpdateProfileCommand command);
        Task<bool> UpdateProfilePictureAsync(UpdateProfilePictureCommand command);
        Task<bool> UpdateUserAsync(UpdateUserCommand user);
        Task<bool> CreatePasswordAsync(SetPasswordCommand request);
        Task<bool> ChangePasswordAsync(ChangePasswordCommand request);
        Task<bool> RemoveAccountAsync(RemoveAccountCommand request);
        Task<bool> HasPassword(string username);
        Task<IEnumerable<User>> GetByIdAsync(params string[] id);
        Task<IEnumerable<User>> GetUsers(PagingViewModel paging);
        Task<User> FindByEmailAsync(string email);
        Task<IDomainUser> FindByNameAsync(string username);
        Task<User> FindByProviderAsync(string provider, string providerUserId);
        Task UpdateUserAsync(User user);
        Task<IEnumerable<Claim>> GetClaimByName(string userName);
        Task<bool> SaveClaim(string username, Claim claim);
        Task<bool> RemoveClaim(string username, string claimType, string value);
        Task<IEnumerable<string>> GetRoles(string userName);
        Task<bool> RemoveRole(string username, string requestRole);
        Task<bool> SaveRole(string username, string role);
        Task<IEnumerable<UserLogin>> GetUserLogins(string userName);
        Task<bool> RemoveLogin(string username, string loginProvider, string providerKey);
        Task<IEnumerable<User>> GetUserFromRole(string role);
        Task<bool> RemoveUserFromRole(string name, string username);
        Task<bool> ResetPasswordAsync(string username, string password);
        Task<string> ResetPassword(string email, string code, string password);
        Task<int> Count(string search);
        Task<string> AddLoginAsync(string email, string provider, string providerId);
        Task<User> FindByUsernameOrEmail(string emailOrUsername);
        Task<int> Count(ICustomQueryable findByEmailNameUsername);
        Task<IEnumerable<User>> Search(ICustomQueryable search);
    }
}