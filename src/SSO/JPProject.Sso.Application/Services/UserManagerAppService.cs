using AspNetCore.IQueryable.Extensions;
using AutoMapper;
using IdentityModel;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.ViewModels;
using JPProject.Sso.Application.EventSourcedNormalizers;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Application.ViewModels.RoleViewModels;
using JPProject.Sso.Application.ViewModels.UserViewModels;
using JPProject.Sso.Domain.Commands.User;
using JPProject.Sso.Domain.Commands.UserManagement;
using JPProject.Sso.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public UserManagerAppService(IMapper mapper,
            IUserService userService,
            IMediatorHandler bus,
            IEventStoreRepository eventStoreRepository,
            IStorage storage
            )
        {
            _mapper = mapper;
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
            return _mapper.Map<UserViewModel>(user);
        }

        public async Task<UserViewModel> FindByEmailAsync(string email)
        {
            var user = await _userService.FindByEmailAsync(email);
            return _mapper.Map<UserViewModel>(user);
        }

        public async Task<UserViewModel> FindByProviderAsync(string provider, string providerUserId)
        {
            var user = await _userService.FindByProviderAsync(provider, providerUserId);
            return _mapper.Map<UserViewModel>(user);
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsersById(params string[] id)
        {
            var users = await _userService.GetByIdAsync(id);
            return _mapper.Map<IEnumerable<UserListViewModel>>(users);
        }

        public async Task<UserViewModel> GetUserDetails(string username)
        {
            var users = await _userService.FindByNameAsync(username);
            return _mapper.Map<UserViewModel>(users);
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

        public async Task<ListOf<UserListViewModel>> SearchUsers(ICustomQueryable search)
        {
            var users = await _userService.Search(search);
            var total = await _userService.Count(search);
            return new ListOf<UserListViewModel>(_mapper.Map<IEnumerable<UserListViewModel>>(users), total);
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
            var command = _mapper.Map<UpdateUserCommand>(model);
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
            await _storage.Remove(model.Username.ToSha256(), "images");
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

