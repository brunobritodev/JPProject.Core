using AutoMapper;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Application.ViewModels.UserViewModels;
using JPProject.Sso.Domain.Commands.User;
using JPProject.Sso.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;

        public UserAppService(IMapper mapper,
            IUserService userService,
            IMediatorHandler bus,
            IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _userService = userService;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
        }

        public Task<bool> Register(RegisterUserViewModel model)
        {
            var registerCommand = _mapper.Map<RegisterNewUserCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> RegisterWithoutPassword(SocialViewModel model)
        {
            var registerCommand = _mapper.Map<RegisterNewUserWithoutPassCommand>(model);
            return Bus.SendCommand(registerCommand);
        }
        public Task<bool> RegisterWithProvider(RegisterUserViewModel model)
        {
            var registerCommand = _mapper.Map<RegisterNewUserWithProviderCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> SendResetLink(ForgotPasswordViewModel model)
        {
            var registerCommand = _mapper.Map<SendResetLinkCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> ResetPassword(ResetPasswordViewModel model)
        {
            var registerCommand = _mapper.Map<ResetPasswordCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> ConfirmEmail(ConfirmEmailViewModel model)
        {
            var registerCommand = _mapper.Map<ConfirmEmailCommand>(model);
            return Bus.SendCommand(registerCommand);
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

        public Task<bool> AddLogin(SocialViewModel model)
        {
            var registerCommand = _mapper.Map<AddLoginCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> CheckUsername(string userName)
        {
            return _userService.UsernameExist(userName);
        }

        public Task<bool> CheckEmail(string email)
        {
            return _userService.EmailExist(email);
        }

        public async Task<RegisterUserViewModel> FindByLoginAsync(string provider, string providerUserId)
        {
            var model = await _userService.FindByLoginAsync(provider, providerUserId);
            return _mapper.Map<RegisterUserViewModel>(model);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
