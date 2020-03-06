using AutoMapper;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.ViewModels;
using JPProject.Sso.Application.AutoMapper;
using JPProject.Sso.Application.EventSourcedNormalizers;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Application.ViewModels.RoleViewModels;
using JPProject.Sso.Application.ViewModels.UserViewModels;
using JPProject.Sso.Domain.Commands.User;
using JPProject.Sso.Domain.Commands.UserManagement;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.ViewModels;
using JPProject.Sso.Domain.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.Services
{
    public class UserManagerAppService : IUserManageAppService
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IStorage _storage;
        private readonly IMediatorHandler Bus;

        public UserManagerAppService(
            IUserService userService,
            IMediatorHandler bus,
            IEventStoreRepository eventStoreRepository,
            IStorage storage
            )
        {
            _mapper = UserMapping.Mapper;
            _userService = userService;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
            _storage = storage;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<UserViewModel> FindByUsernameAsync(string username)
        {
            var user = await _userService.FindByNameAsync(username);
            var userVo = _mapper.Map<UserViewModel>(user);
            return await GetUserMetadata(userVo);
        }

        private async Task<UserViewModel> GetUserMetadata(UserViewModel user)
        {
            if (string.IsNullOrEmpty(user?.UserName))
                return null;
            var claims = await _userService.GetClaimByName(user?.UserName);
            user?.UpdateMetadata(claims.ToList());
            return user;
        }

        public async Task<UserViewModel> FindByEmailAsync(string email)
        {
            var user = await _userService.FindByEmailAsync(email);
            var userVo = _mapper.Map<UserViewModel>(user);
            return await GetUserMetadata(userVo);
        }

        public async Task<UserViewModel> FindByProviderAsync(string provider, string providerUserId)
        {
            var user = await _userService.FindByProviderAsync(provider, providerUserId);
            var userVo = _mapper.Map<UserViewModel>(user);
            return await GetUserMetadata(userVo);
        }

        public async Task<UserViewModel> GetUserDetails(string username)
        {
            var users = await _userService.FindByNameAsync(username);
            var userVo = _mapper.Map<UserViewModel>(users);
            return await GetUserMetadata(userVo);
        }

        public async Task<IEnumerable<ClaimViewModel>> GetClaims(string userName)
        {
            return _mapper.Map<IEnumerable<ClaimViewModel>>(await _userService.GetClaimByName(userName));
        }

        public async Task<IEnumerable<RoleViewModel>> GetRoles(string userName)
        {
            var roles = await _userService.GetRoles(userName);
            return roles.Select(s => new RoleViewModel() { Name = s });
        }

        public async Task<IEnumerable<UserLoginViewModel>> GetLogins(string userName)
        {
            return _mapper.Map<IEnumerable<UserLoginViewModel>>(await _userService.GetUserLogins(userName));
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsersInRole(string role)
        {
            return _mapper.Map<IEnumerable<UserListViewModel>>(await _userService.GetUserFromRole(role));
        }

        public async Task<ListOf<EventHistoryData>> GetEvents(string username, PagingViewModel paging)
        {
            var history = await _eventStoreRepository.GetEvents(username, paging);
            var total = await _eventStoreRepository.Count(username, paging.Search);
            return new ListOf<EventHistoryData>(_mapper.Map<IEnumerable<EventHistoryData>>(history), total);
        }

        public Task<bool> HasPassword(string username)
        {
            return _userService.HasPassword(username);
        }

        public async Task<ListOf<UserListViewModel>> SearchUsers(IUserSearch search)
        {
            var users = _mapper.Map<IEnumerable<UserListViewModel>>(await _userService.Search(search));
            var total = await _userService.Count(search);

            var claims = await GetClaimsFromUsers(users.Select(s => s.UserName), JwtClaimTypes.Picture, JwtClaimTypes.GivenName);
            foreach (var domainUser in users)
            {
                if (claims.ContainsKey(domainUser.UserName))
                    domainUser.UpdateMetadata(claims[domainUser.UserName]);
            }
            return new ListOf<UserListViewModel>(_mapper.Map<IEnumerable<UserListViewModel>>(users), total);
        }

        public async Task<ListOf<UserListViewModel>> SearchUsersByClaims(IUserClaimSearch search)
        {
            var users = _mapper.Map<IEnumerable<UserListViewModel>>(await _userService.SearchByClaim(search));
            var total = await _userService.CountByClaim(search);

            var claims = await GetClaimsFromUsers(users.Select(s => s.UserName), JwtClaimTypes.Picture, JwtClaimTypes.GivenName);
            foreach (var domainUser in users)
            {
                if (claims.ContainsKey(domainUser.UserName))
                    domainUser.UpdateMetadata(claims[domainUser.UserName]);
            }
            return new ListOf<UserListViewModel>(_mapper.Map<IEnumerable<UserListViewModel>>(users), total);
        }

        public async Task<Dictionary<Username, IEnumerable<Claim>>> GetClaimsFromUsers(IEnumerable<string> usernames, params string[] claimType)
        {
            return await _userService.GetClaimsFromUsers(usernames, claimType);
        }

        public Task<bool> CreatePassword(SetPasswordViewModel model)
        {
            var registerCommand = _mapper.Map<SetPasswordCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> ChangePassword(ChangePasswordViewModel model)
        {
            var registerCommand = _mapper.Map<ChangePasswordCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> UpdateUser(UserViewModel model)
        {
            var command = _mapper.Map<AdminUpdateUserCommand>(model);
            return Bus.SendCommand(command);
        }

        public Task<bool> SaveRole(SaveUserRoleViewModel model)
        {
            var registerCommand = _mapper.Map<SaveUserRoleCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> SaveClaim(SaveUserClaimViewModel model)
        {
            var registerCommand = _mapper.Map<SaveUserClaimCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> UpdateProfile(UserViewModel model)
        {
            var registerCommand = _mapper.Map<UpdateProfileCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public async Task<bool> UpdateProfilePicture(ProfilePictureViewModel model)
        {
            await _storage.Remove(model.Filename, "images");
            model.Picture = await _storage.Upload(model);
            var updateCommand = _mapper.Map<UpdateProfilePictureCommand>(model);
            return await Bus.SendCommand(updateCommand);
        }

        public Task<bool> RemoveAccount(RemoveAccountViewModel model)
        {
            var removeCommand = _mapper.Map<RemoveAccountCommand>(model);
            return Bus.SendCommand(removeCommand);
        }

        public Task<bool> RemoveClaim(RemoveUserClaimViewModel model)
        {
            var removeCommand = _mapper.Map<RemoveUserClaimCommand>(model);
            return Bus.SendCommand(removeCommand);
        }

        public Task<bool> RemoveRole(RemoveUserRoleViewModel model)
        {
            var removeCommand = _mapper.Map<RemoveUserRoleCommand>(model);
            return Bus.SendCommand(removeCommand);
        }

        public Task<bool> RemoveLogin(RemoveUserLoginViewModel model)
        {
            var registerCommand = _mapper.Map<RemoveUserLoginCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> ResetPassword(AdminChangePasswordViewodel model)
        {
            var registerCommand = _mapper.Map<AdminChangePasswordCommand>(model);
            return Bus.SendCommand(registerCommand);
        }
    }
}

