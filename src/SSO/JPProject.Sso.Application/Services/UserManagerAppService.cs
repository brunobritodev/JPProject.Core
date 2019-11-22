using AutoMapper;
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
        private readonly IImageStorage _imageStorage;
        private readonly IMediatorHandler Bus;

        public UserManagerAppService(IMapper mapper,
            IUserService userService,
            IMediatorHandler bus,
            IEventStoreRepository eventStoreRepository,
            IImageStorage imageStorage
            )
        {
            _mapper = mapper;
            _userService = userService;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
            _imageStorage = imageStorage;
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task UpdateProfile(UserViewModel model)
        {
            var registerCommand = _mapper.Map<UpdateProfileCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public async Task UpdateProfilePicture(ProfilePictureViewModel model)
        {
            model.Picture = await _imageStorage.SaveAsync(model);
            var updateCommand = _mapper.Map<UpdateProfilePictureCommand>(model);
            await Bus.SendCommand(updateCommand);
        }

        public Task CreatePassword(SetPasswordViewModel model)
        {
            var registerCommand = _mapper.Map<SetPasswordCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task RemoveAccount(RemoveAccountViewModel model)
        {
            var removeCommand = _mapper.Map<RemoveAccountCommand>(model);
            return Bus.SendCommand(removeCommand);
        }

        public Task ChangePassword(ChangePasswordViewModel model)
        {
            var registerCommand = _mapper.Map<ChangePasswordCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> HasPassword(string userId)
        {
            return _userService.HasPassword(userId);
        }

        public async Task<ListOf<EventHistoryData>> GetEvents(string username, PagingViewModel paging)
        {
            var history = await _eventStoreRepository.GetEvents(username, paging);
            var total = await _eventStoreRepository.Count(username, paging.Search);
            return new ListOf<EventHistoryData>(_mapper.Map<IEnumerable<EventHistoryData>>(history), total);
        }

        public async Task<ListOf<UserListViewModel>> GetUsers(PagingViewModel paging)
        {
            var users = await _userService.GetUsers(paging);
            var total = await _userService.Count(paging.Search);
            return new ListOf<UserListViewModel>(_mapper.Map<IEnumerable<UserListViewModel>>(users), total);
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

        public async Task<UserViewModel> GetUserAsync(string value)
        {
            var users = await _userService.FindByUserId(value);
            return _mapper.Map<UserViewModel>(users);
        }

        public Task UpdateUser(UserViewModel model)
        {
            var command = _mapper.Map<UpdateUserCommand>(model);
            return Bus.SendCommand(command);
        }

        public async Task<IEnumerable<ClaimViewModel>> GetClaims(string userName)
        {
            return _mapper.Map<IEnumerable<ClaimViewModel>>(await _userService.GetClaimByName(userName));
        }

        public Task SaveClaim(SaveUserClaimViewModel model)
        {
            var registerCommand = _mapper.Map<SaveUserClaimCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task RemoveClaim(RemoveUserClaimViewModel model)
        {
            var removeCommand = _mapper.Map<RemoveUserClaimCommand>(model);
            return Bus.SendCommand(removeCommand);
        }

        public async Task<IEnumerable<RoleViewModel>> GetRoles(string userName)
        {
            var roles = await _userService.GetRoles(userName);
            return roles.Select(s => new RoleViewModel() { Name = s });
        }

        public Task RemoveRole(RemoveUserRoleViewModel model)
        {
            var removeCommand = _mapper.Map<RemoveUserRoleCommand>(model);
            return Bus.SendCommand(removeCommand);
        }

        public Task SaveRole(SaveUserRoleViewModel model)
        {
            var registerCommand = _mapper.Map<SaveUserRoleCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public async Task<IEnumerable<UserLoginViewModel>> GetLogins(string userName)
        {
            return _mapper.Map<IEnumerable<UserLoginViewModel>>(await _userService.GetUserLogins(userName));
        }

        public Task RemoveLogin(RemoveUserLoginViewModel model)
        {
            var registerCommand = _mapper.Map<RemoveUserLoginCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsersInRole(string role)
        {
            return _mapper.Map<IEnumerable<UserListViewModel>>(await _userService.GetUserFromRole(role));
        }

        public Task ResetPassword(AdminChangePasswordViewodel model)
        {
            var registerCommand = _mapper.Map<AdminChangePasswordCommand>(model);
            return Bus.SendCommand(registerCommand);
        }
    }
}

