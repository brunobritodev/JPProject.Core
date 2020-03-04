using AutoMapper;
using JPProject.Domain.Core.Events;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.ViewModels;
using JPProject.Sso.Application.EventSourcedNormalizers;
using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Application.ViewModels.UserViewModels;
using JPProject.Sso.Domain.Commands.User;
using JPProject.Sso.Domain.Commands.UserManagement;
using JPProject.Sso.Domain.Models;
using System.Globalization;
using System.Security.Claims;

namespace JPProject.Sso.Application.AutoMapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            /*
          * User Creation Commands
          */
            CreateMap<RegisterUserViewModel, RegisterNewUserCommand>().ConstructUsing(c => new RegisterNewUserCommand(c.Username, c.Email, c.Name, c.PhoneNumber, c.Password, c.ConfirmPassword, c.Birthdate, c.SocialNumber, true));
            CreateMap<AdminRegisterUserViewModel, RegisterNewUserCommand>().ConstructUsing(c => new RegisterNewUserCommand(c.Username, c.Email, c.Name, c.PhoneNumber, c.Password, c.ConfirmPassword, c.Birthdate, c.SocialNumber, c.ConfirmEmail));
            CreateMap<SocialViewModel, RegisterNewUserWithoutPassCommand>(MemberList.Source).ConstructUsing(c => new RegisterNewUserWithoutPassCommand(c.Email, c.Email, c.Name, c.Picture, c.Provider, c.ProviderId));

            CreateMap<RegisterUserViewModel, RegisterNewUserWithProviderCommand>().ConstructUsing(c => new RegisterNewUserWithProviderCommand(c.Username, c.Email, c.Name, c.PhoneNumber, c.Password, c.ConfirmPassword, c.Picture, c.Provider, c.ProviderId, c.Birthdate, c.SocialNumber));
            CreateMap<ForgotPasswordViewModel, SendResetLinkCommand>().ConstructUsing(c => new SendResetLinkCommand(c.UsernameOrEmail));
            CreateMap<ResetPasswordViewModel, ResetPasswordCommand>().ConstructUsing(c => new ResetPasswordCommand(c.Password, c.ConfirmPassword, c.Code, c.Email));
            CreateMap<ConfirmEmailViewModel, ConfirmEmailCommand>().ConstructUsing(c => new ConfirmEmailCommand(c.Code, c.Email));
            CreateMap<SocialViewModel, AddLoginCommand>().ConstructUsing(c => new AddLoginCommand(c.Email, c.Provider, c.ProviderId));
            /*
             * User Management commands
             */
            CreateMap<UserViewModel, UpdateProfileCommand>().ConstructUsing(c => new UpdateProfileCommand(c.UserName, c.Url, c.Bio, c.Company, c.JobTitle, c.Name, c.PhoneNumber, c.SocialNumber, c.Birthdate));
            CreateMap<UserViewModel, AdminUpdateUserCommand>().ConstructUsing(c => new AdminUpdateUserCommand(c.Email, c.UserName, c.Name, c.PhoneNumber, c.EmailConfirmed, c.PhoneNumberConfirmed, c.TwoFactorEnabled, c.LockoutEnd, c.LockoutEnabled, c.AccessFailedCount, c.Birthdate, c.SocialNumber));
            CreateMap<ProfilePictureViewModel, UpdateProfilePictureCommand>().ConstructUsing(c => new UpdateProfilePictureCommand(c.Username, c.Picture));
            CreateMap<ChangePasswordViewModel, ChangePasswordCommand>().ConstructUsing(c => new ChangePasswordCommand(c.Username, c.OldPassword, c.NewPassword, c.ConfirmPassword));
            CreateMap<SetPasswordViewModel, SetPasswordCommand>().ConstructUsing(c => new SetPasswordCommand(c.Username, c.NewPassword, c.ConfirmPassword));
            CreateMap<RemoveAccountViewModel, RemoveAccountCommand>().ConstructUsing(c => new RemoveAccountCommand(c.Username));
            CreateMap<SaveUserClaimViewModel, SaveUserClaimCommand>().ConstructUsing(c => new SaveUserClaimCommand(c.Username, c.Type, c.Value));
            CreateMap<RemoveUserClaimViewModel, RemoveUserClaimCommand>().ConstructUsing(c => new RemoveUserClaimCommand(c.Username, c.Type, c.Value));
            CreateMap<RemoveUserRoleViewModel, RemoveUserRoleCommand>().ConstructUsing(c => new RemoveUserRoleCommand(c.Username, c.Role));
            CreateMap<SaveUserRoleViewModel, SaveUserRoleCommand>().ConstructUsing(c => new SaveUserRoleCommand(c.Username, c.Role));
            CreateMap<RemoveUserLoginViewModel, RemoveUserLoginCommand>().ConstructUsing(c => new RemoveUserLoginCommand(c.Username, c.LoginProvider, c.ProviderKey));
            CreateMap<AdminChangePasswordViewodel, AdminChangePasswordCommand>().ConstructUsing(c => new AdminChangePasswordCommand(c.Password, c.ConfirmPassword, c.Username));

            /*
             * Domain to view model
             */
            CreateMap<IDomainUser, UserViewModel>(MemberList.Destination);
            CreateMap<IDomainUser, UserListViewModel>(MemberList.Destination);
            CreateMap<UserLogin, UserLoginViewModel>(MemberList.Destination);
            CreateMap<StoredEvent, EventHistoryData>().ConstructUsing(a => new EventHistoryData(a.Message, a.Id.ToString(), a.Details, a.Timestamp.ToString(CultureInfo.InvariantCulture), a.User, a.MessageType, a.RemoteIpAddress));
            CreateMap<Claim, ClaimViewModel>().ConstructUsing(a => new ClaimViewModel(a.Type, a.Value));

        }
    }
}