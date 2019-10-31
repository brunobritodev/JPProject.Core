using JPProject.Domain.Core.ViewModels;
using JPProject.Sso.Application.EventSourcedNormalizers;
using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Application.ViewModels.RoleViewModels;
using JPProject.Sso.Application.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.Interfaces
{
    public interface IUserManageAppService : IDisposable
    {
        Task UpdateProfile(UserViewModel model);
        Task UpdateProfilePicture(ProfilePictureViewModel model);
        Task ChangePassword(ChangePasswordViewModel model);
        Task CreatePassword(SetPasswordViewModel model);
        Task RemoveAccount(RemoveAccountViewModel model);
        Task<bool> HasPassword(string userId);
        Task<ListOf<EventHistoryData>> GetEvents(string username, PagingViewModel paging);

        Task<UserViewModel> GetUserDetails(string username);
        Task<UserViewModel> GetUserAsync(string value);
        Task UpdateUser(UserViewModel model);

        Task<IEnumerable<ClaimViewModel>> GetClaims(string userName);
        Task SaveClaim(SaveUserClaimViewModel model);
        Task RemoveClaim(RemoveUserClaimViewModel model);
        Task<IEnumerable<RoleViewModel>> GetRoles(string userName);
        Task RemoveRole(RemoveUserRoleViewModel model);
        Task SaveRole(SaveUserRoleViewModel model);
        Task<IEnumerable<UserLoginViewModel>> GetLogins(string userName);
        Task RemoveLogin(RemoveUserLoginViewModel model);
        Task<IEnumerable<UserListViewModel>> GetUsersInRole(string role);
        Task ResetPassword(AdminChangePasswordViewodel model);
        Task<ListOf<UserListViewModel>> GetUsers(PagingViewModel page);
        Task<IEnumerable<UserListViewModel>> GetUsersById(params string[] id);
    }
}
