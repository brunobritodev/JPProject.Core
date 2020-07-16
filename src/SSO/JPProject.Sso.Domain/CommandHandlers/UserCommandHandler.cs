using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Commands;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using JPProject.Domain.Core.Util;
using JPProject.Sso.Domain.Commands.User;
using JPProject.Sso.Domain.Events.User;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Domain.ViewModels.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace JPProject.Sso.Domain.CommandHandlers
{
    public class UserCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewUserCommand, bool>,
        IRequestHandler<RegisterNewUserWithoutPassCommand, bool>,
        IRequestHandler<RegisterNewUserWithProviderCommand, bool>,
        IRequestHandler<SendResetLinkCommand, bool>,
        IRequestHandler<ResetPasswordCommand, bool>,
        IRequestHandler<ConfirmEmailCommand, bool>,
        IRequestHandler<AddLoginCommand, bool>
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IEmailRepository _emailRepository;

        public UserCommandHandler(
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            IUserService userService,
            IEmailService emailService,
            IEmailRepository emailRepository) : base(uow, bus, notifications)
        {
            _userService = userService;
            _emailService = emailService;
            _emailRepository = emailRepository;
        }


        public async Task<bool> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var emailAlreadyExist = await _userService.FindByEmailAsync(request.Email);
            if (emailAlreadyExist != null)
            {
                await Bus.RaiseEvent(new DomainNotification("New User", "E-mail already exist. If you don't remember your passwork, reset it."));
                return false;
            }

            var usernameAlreadyExist = await _userService.FindByNameAsync(request.Username);
            if (usernameAlreadyExist != null)
            {
                await Bus.RaiseEvent(new DomainNotification("New User", "Username already exist. If you don't remember your passwork, reset it."));
                return false;
            }

            var result = await _userService.CreateUserWithPass(request, request.Password);
            if (result.HasValue)
            {
                var user = await _userService.FindByNameAsync(request.Username);
                await SendEmailToUser(user, request, result.Value, EmailType.NewUser);
                await Bus.RaiseEvent(new UserRegisteredEvent(result.Value.Username, user.Email));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(RegisterNewUserWithoutPassCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var emailAlreadyExist = await _userService.FindByEmailAsync(request.Email);
            if (emailAlreadyExist != null)
            {
                await Bus.RaiseEvent(new DomainNotification("New User", "E-mail already exist. If you don't remember your passwork, reset it."));
                return false;
            }
            var usernameAlreadyExist = await _userService.FindByNameAsync(request.Username);

            if (usernameAlreadyExist != null)
            {
                await Bus.RaiseEvent(new DomainNotification("New User", "Username already exist. If you don't remember your passwork, reset it."));
                return false;
            }

            var result = await _userService.CreateUserWithouthPassword(request);

            if (result.HasValue)
            {
                var user = await _userService.FindByNameAsync(request.Username);
                await SendEmailToUser(user, request, result.Value, EmailType.NewUserWithoutPassword);
                await Bus.RaiseEvent(new UserRegisteredEvent(result.Value.Username, user.Email));
                return true;
            }
            return false;
        }


        public async Task<bool> Handle(RegisterNewUserWithProviderCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false; ;
            }

            var result = await _userService.CreateUserWithProviderAndPass(request);
            if (result.HasValue)
            {
                var user = await _userService.FindByNameAsync(request.Username);
                await SendEmailToUser(user, request, result.Value, EmailType.NewUser);
                await Bus.RaiseEvent(new UserRegisteredEvent(request.Username, user.Email));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(SendResetLinkCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var accountResult = await _userService.GenerateResetPasswordLink(request.EmailOrUsername);

            if (accountResult.HasValue)
            {
                var user = await _userService.FindByUsernameOrEmail(request.EmailOrUsername);
                await SendEmailToUser(user, request, accountResult.Value, EmailType.RecoverPassword);
                await Bus.RaiseEvent(new ResetLinkGeneratedEvent(request.Email, request.Username));
                return true;
            }
            return false;
        }

        private async Task SendEmailToUser(IDomainUser user, UserCommand request, AccountResult accountResult, EmailType type)
        {
            if (user.EmailConfirmed || !user.Email.IsEmail())
                return;

            var email = await _emailRepository.GetByType(type);
            if (email is null)
                return;

            var claims = await _userService.GetClaimByName(user.UserName);
            await _emailService.SendEmailAsync(email.GetMessage(user, accountResult, request, claims));
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var emailSent = await _userService.ResetPassword(request.Email, request.Code, request.Password);

            if (emailSent != null)
            {
                await Bus.RaiseEvent(new AccountPasswordResetedEvent(emailSent, request.Email, request.Code));
                return true;
            }
            return false;
        }
        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var result = await _userService.ConfirmEmailAsync(request.Email, request.Code);
            if (result != null)
            {
                await Bus.RaiseEvent(new EmailConfirmedEvent(request.Email, request.Code, result));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(AddLoginCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var result = await _userService.AddLoginAsync(request.Email, request.Provider, request.ProviderId);
            if (result != null)
            {
                await Bus.RaiseEvent(new NewLoginAddedEvent(result, request.Email, request.Provider, request.ProviderId));
                return true;
            }
            return false;
        }

    }
}
