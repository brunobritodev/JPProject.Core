using AspNetCore.IQueryable.Extensions;
using AspNetCore.IQueryable.Extensions.Filter;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using JPProject.Domain.Core.StringUtils;
using JPProject.Sso.Domain.Commands.User;
using JPProject.Sso.Domain.Commands.UserManagement;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Domain.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JPProject.Sso.Infra.Identity.Services
{
    public class UserService<TUser, TKey> : IUserService
        where TUser : IdentityUser<TKey>, IDomainUser
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;
        private readonly IMediatorHandler _bus;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly IIdentityFactory<TUser> _userFactory;

        public UserService(
            UserManager<TUser> userManager,
            IMediatorHandler bus,
            ILoggerFactory loggerFactory,
            IConfiguration config,
            IIdentityFactory<TUser> userFactory)
        {
            _userManager = userManager;
            _bus = bus;
            _config = config;
            _userFactory = userFactory;
            _logger = loggerFactory.CreateLogger<UserService<TUser, TKey>>();
        }

        public Task<AccountResult?> CreateUserWithPass(RegisterNewUserCommand command, string password)
        {
            var user = _userFactory.Create(command);
            return CreateUser(user, password, null, null);
        }

        public Task<AccountResult?> CreateUserWithProvider(RegisterNewUserWithoutPassCommand command, string provider, string providerUserId)
        {
            var user = _userFactory.Create(command);
            return CreateUser(user, null, provider, providerUserId);
        }

        public Task<AccountResult?> CreateUserWithProviderAndPass(RegisterNewUserWithProviderCommand command)
        {
            var user = _userFactory.Create(command);
            return CreateUser(user, command.Password, command.Provider, command.ProviderId);
        }

        private async Task<AccountResult?> CreateUser(TUser user, string password, string provider, string providerId)
        {
            IdentityResult result;

            if (provider.IsPresent())
            {
                var userByProvider = await _userManager.FindByLoginAsync(provider, providerId);
                if (userByProvider != null)
                    await _bus.RaiseEvent(new DomainNotification("New User", $"User already taken with {provider}"));
            }

            if (password.IsMissing())
                result = await _userManager.CreateAsync(user);
            else
                result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // User claim for write customers data
                //await _userManager.AddClaimAsync(newUser, new Claim("User", "Write"));

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = $"{_config.GetValue<string>("ApplicationSettings:UserManagementURL")}/confirm-email?userId={ user.Email.UrlEncode()}&code={code.UrlEncode()}";

                await AddClaims(user);

                if (!string.IsNullOrWhiteSpace(provider))
                    await AddLoginAsync(user, provider, providerId);


                if (password.IsPresent())
                    _logger.LogInformation("User created a new account with password.");

                if (provider.IsPresent())
                    _logger.LogInformation($"Provider {provider} associated.");
                return new AccountResult(user.UserName, code, callbackUrl);
            }

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return null;
        }

        /// <summary>
        /// Add custom claims here
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Task AddClaims(TUser user)
        {
            return Task.CompletedTask;
            //var claims = new List<Claim>();
            //claims.Add(new Claim("custom_claim_name", "any value"));
            //claims.Add(new Claim(JwtClaimTypes.Picture, user.Picture));
            //var result = await _userManager.AddClaimsAsync(user, claims);

            //if (result.Succeeded)
            //{
            //    _logger.LogInformation("Claim created successfull.");
            //}
            //else
            //{
            //    foreach (var error in result.Errors)
            //    {
            //        await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            //    }
            //}
        }

        public async Task<bool> UsernameExist(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user != null;
        }

        public async Task<bool> EmailExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }


        public async Task<AccountResult?> GenerateResetPasswordLink(string emailOrUsername)
        {
            var user = await GetUserByEmailOrUsername(emailOrUsername);
            if (user == null)
                return null;


            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = $"{_config.GetValue<string>("ApplicationSettings:UserManagementURL")}/reset-password?email={user.Email.UrlEncode()}&code={code.UrlEncode()}";

            //await _emailService.SendEmailAsync(user.Email, "Reset Password", $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
            //_logger.LogInformation("Reset link sended to userId.");

            return new AccountResult(user.UserName, code, callbackUrl);
        }

        public async Task<string> ConfirmEmailAsync(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                await _bus.RaiseEvent(new DomainNotification("Email", $"Unable to load userId with ID '{email}'."));
                return null;
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return user.UserName;

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return null;
        }



        public async Task<bool> UpdateProfileAsync(UpdateProfileCommand command)
        {
            var user = await _userManager.FindByNameAsync(command.Username);
            _userFactory.UpdateProfile(command, user);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return true;

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return false;
        }

        public async Task<bool> UpdateProfilePictureAsync(UpdateProfilePictureCommand command)
        {
            var user = await _userManager.FindByNameAsync(command.Username);
            var domainUser = user as IDomainUser;
            domainUser.UpdatePicture(command.Picture);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return true;

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return false;
        }


        public async Task<bool> UpdateUserAsync(AdminUpdateUserCommand command)
        {
            var userDb = await _userManager.FindByNameAsync(command.Username);
            _userFactory.UpdateInfo(command, userDb);

            var resut = await _userManager.UpdateAsync(userDb);
            if (!resut.Succeeded)
            {
                foreach (var error in resut.Errors)
                {
                    await _bus.RaiseEvent(new DomainNotification("User", error.Description));
                }

                return false;
            }

            return true;
        }

        public async Task<bool> CreatePasswordAsync(SetPasswordCommand request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (hasPassword)
            {
                /*
                 * DO NOT display the reason.
                 * if this happen is because userId are trying to hack.
                 */
                throw new Exception("Unknown error");
            }

            var result = await _userManager.AddPasswordAsync(user, request.Password);
            if (result.Succeeded)
                return true;

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return false;
        }

        public async Task<bool> RemoveAccountAsync(RemoveAccountCommand request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return true;

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return false;
        }

        public async Task<bool> HasPassword(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            return await _userManager.HasPasswordAsync(user);
        }

        private IDomainUser GetUser(TUser s)
        {
            return _userFactory.ToDomainUser(s);
        }

        private async Task AddLoginAsync(TUser user, string provider, string providerUserId)
        {
            var result = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }
        }

        public async Task<IDomainUser> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return GetUser(user);
        }

        public async Task<IDomainUser> FindByNameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return GetUser(user);
        }

        public async Task<IDomainUser> FindByProviderAsync(string provider, string providerUserId)
        {
            var user = await _userManager.FindByLoginAsync(provider, providerUserId);
            return GetUser(user);
        }

        public async Task<IEnumerable<Claim>> GetClaimByName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var claims = await _userManager.GetClaimsAsync(user);

            return claims;
        }

        public async Task<bool> SaveClaim(string username, Claim claim)
        {
            var user = await _userManager.FindByNameAsync(username);
            var result = await _userManager.AddClaimAsync(user, claim);

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return result.Succeeded;
        }

        public async Task<bool> RemoveClaim(string username, string claimType, string value)
        {
            var user = await _userManager.FindByNameAsync(username);
            var claims = await _userManager.GetClaimsAsync(user);

            var claimToRemove = value.IsMissing() ?
                                    claims.First(c => c.Type.Equals(claimType)) :
                                    claims.First(c => c.Type.Equals(claimType) && c.Value.Equals(value));

            var result = await _userManager.RemoveClaimAsync(user, claimToRemove);

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return result.Succeeded;
        }

        public async Task<IEnumerable<string>> GetRoles(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> RemoveRole(string username, string requestRole)
        {
            var user = await _userManager.FindByNameAsync(username);
            var result = await _userManager.RemoveFromRoleAsync(user, requestRole);

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return result.Succeeded;
        }


        public async Task<bool> SaveRole(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            var result = await _userManager.AddToRoleAsync(user, role);

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return result.Succeeded;
        }

        public async Task<IEnumerable<UserLogin>> GetUserLogins(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var logins = await _userManager.GetLoginsAsync(user);
            return logins.Select(a => new UserLogin(a.LoginProvider, a.ProviderDisplayName, a.ProviderKey));
        }

        public async Task<bool> RemoveLogin(string username, string loginProvider, string providerKey)
        {
            var user = await _userManager.FindByNameAsync(username);
            var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return result.Succeeded;
        }

        public async Task<IEnumerable<IDomainUser>> GetUserFromRole(string role)
        {
            return (await _userManager.GetUsersInRoleAsync(role));//.Select(GetUser);
        }

        public async Task<bool> RemoveUserFromRole(string name, string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var result = await _userManager.RemoveFromRoleAsync(user, name);
            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return result.Succeeded;
        }

        public async Task<bool> ResetPasswordAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, password);
            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return result.Succeeded;
        }

        public async Task<string> ResetPassword(string email, string code, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Don't reveal that the userId does not exist
                return null;
            }

            var result = await _userManager.ResetPasswordAsync(user, code, password);

            if (result.Succeeded)
            {
                var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                if (!emailConfirmed)
                {
                    user.ConfirmEmail();
                    await _userManager.UpdateAsync(user);

                }
                _logger.LogInformation("Password reseted successfull.");
                return user.UserName;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
                }
            }

            return null;
        }


        public async Task<bool> ChangePasswordAsync(ChangePasswordCommand request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.Password);
            if (result.Succeeded)
                return true;

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return false;
        }

        public async Task<string> AddLoginAsync(string email, string provider, string providerId)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            await AddLoginAsync(user, provider, providerId);

            return user.UserName;
        }

        public async Task<IDomainUser> FindByUsernameOrEmail(string emailOrUsername)
        {
            var user = await GetUserByEmailOrUsername(emailOrUsername);
            return GetUser(user);
        }

        private async Task<TUser> GetUserByEmailOrUsername(string emailOrUsername)
        {
            TUser user;
            if (emailOrUsername.IsEmail())
                user = await _userManager.FindByEmailAsync(emailOrUsername);
            else
                user = await _userManager.FindByNameAsync(emailOrUsername);
            return user;
        }
        public Task<int> Count(ICustomQueryable search)
        {
            return _userManager.Users.Filter(search).CountAsync();
        }

        public async Task<IEnumerable<IDomainUser>> Search(ICustomQueryable search)
        {
            var users = await _userManager.Users.Apply(search).ToListAsync();
            return users.Select(GetUser);
        }
    }
}
