using AspNetCore.IQueryable.Extensions;
using AspNetCore.IQueryable.Extensions.Filter;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using JPProject.Domain.Core.StringUtils;
using JPProject.Domain.Core.ViewModels;
using JPProject.Sso.Domain.Commands.UserManagement;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Domain.ViewModels.User;
using JPProject.Sso.Infra.Identity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JPProject.Sso.Infra.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IMediatorHandler _bus;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public UserService(
            UserManager<UserIdentity> userManager,
            IMediatorHandler bus,
            ILoggerFactory loggerFactory,
            IConfiguration config)
        {
            _userManager = userManager;
            _bus = bus;
            _config = config;
            _logger = loggerFactory.CreateLogger<UserService>();
        }

        public Task<AccountResult?> CreateUserWithPass(IDomainUser user, string password)
        {
            return CreateUser(user, password, null, null);
        }

        public Task<AccountResult?> CreateUserWithProvider(IDomainUser user, string provider, string providerUserId)
        {
            return CreateUser(user, null, provider, providerUserId);
        }

        public Task<AccountResult?> CreateUserWithProviderAndPass(IDomainUser user, string password, string provider, string providerId)
        {
            return CreateUser(user, password, provider, providerId);
        }

        private async Task<AccountResult?> CreateUser(IDomainUser user, string password, string provider, string providerId)
        {
            var newUser = new UserIdentity
            {
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserName = user.UserName,
                Name = user.Name,
                Picture = user.Picture,
                EmailConfirmed = user.EmailConfirmed,
                SocialNumber = user.SocialNumber,
                Birthdate = user.Birthdate,
                Bio = user.Bio,
                JobTitle = user.JobTitle,
                LockoutEnd = null,
                Company = user.Company,
            };
            IdentityResult result;

            if (provider.IsPresent())
            {
                var userByProvider = await _userManager.FindByLoginAsync(provider, providerId);
                if (userByProvider != null)
                    await _bus.RaiseEvent(new DomainNotification("New User", $"User already taken with {provider}"));
            }

            if (password.IsMissing())
                result = await _userManager.CreateAsync(newUser);
            else
                result = await _userManager.CreateAsync(newUser, password);

            if (result.Succeeded)
            {
                // User claim for write customers data
                //await _userManager.AddClaimAsync(newUser, new Claim("User", "Write"));

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var callbackUrl = $"{_config.GetValue<string>("ApplicationSettings:UserManagementURL")}/confirm-email?userId={ user.Email.UrlEncode()}&code={code.UrlEncode()}";

                await AddClaims(newUser);

                if (!string.IsNullOrWhiteSpace(provider))
                    await AddLoginAsync(newUser, provider, providerId);


                if (password.IsPresent())
                    _logger.LogInformation("User created a new account with password.");

                if (provider.IsPresent())
                    _logger.LogInformation($"Provider {provider} associated.");
                return new AccountResult(newUser.Id, code, callbackUrl);
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
        private Task AddClaims(UserIdentity user)
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

            return new AccountResult(user.Id, code, callbackUrl);
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
                return user.Id;

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return null;
        }



        public async Task<bool> UpdateProfileAsync(UpdateProfileCommand command)
        {
            var user = await _userManager.FindByIdAsync(command.Id);
            user.UpdateBio(command);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                if (!user.Name.Equals(command.Name))
                    await AddOrUpdateClaimAsync(user, claims, "name", user.Name);

                return true;
            }

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return false;
        }

        public async Task<bool> UpdateProfilePictureAsync(UpdateProfilePictureCommand command)
        {
            var user = await _userManager.FindByIdAsync(command.Id);

            user.Picture = command.Picture;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return true;

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return false;
        }

        private async Task AddOrUpdateClaimAsync(UserIdentity user, IEnumerable<Claim> claims, string key, string value)
        {
            var customClaim = claims.FirstOrDefault(a => a.Type == key);
            if (customClaim != null)
            {
                if (customClaim.Value.NotEqual(value))
                    await _userManager.RemoveClaimAsync(user, customClaim);
            }
            else
            {
                await _userManager.AddClaimAsync(user, new Claim(key, value));
            }
        }

        public async Task<bool> UpdateUserAsync(UpdateUserCommand user)
        {
            var userDb = await _userManager.FindByNameAsync(user.Username);
            userDb.UpdateInfo(user);
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
            var user = await _userManager.FindByIdAsync(request.Id);

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

        public async Task<bool> ChangePasswordAsync(ChangePasswordCommand request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.Password);
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
            var user = await _userManager.FindByIdAsync(request.Id);
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return true;

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return false;
        }

        public async Task<bool> HasPassword(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return await _userManager.HasPasswordAsync(user);
        }

        public async Task<IEnumerable<User>> GetByIdAsync(params string[] id)
        {
            var users = await _userManager.Users.Where(w => id.Contains(w.Id)).ToListAsync();

            return users.Select(GetUser).ToList();
        }

        private static User GetUser(UserIdentity s)
        {
            return s?.ToUser();
        }

        public async Task<IEnumerable<User>> GetUsers(PagingViewModel paging)
        {
            List<UserIdentity> users;
            if (paging.Search.IsPresent())
                users = await _userManager.Users.Where(UserFind(paging.Search)).Skip(paging.Offset).Take(paging.Limit).ToListAsync();
            else
                users = await _userManager.Users.Skip(paging.Offset).Take(paging.Limit).ToListAsync();
            return users.Select(GetUser);
        }

        private static Expression<Func<UserIdentity, bool>> UserFind(string search)
        {
            return w => w.UserName.Contains(search) ||
                        w.Email.Contains(search) ||
                        w.Name.Contains(search);
        }

        private async Task AddLoginAsync(UserIdentity user, string provider, string providerUserId)
        {
            var result = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return GetUser(user);
        }

        public async Task<User> FindByNameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return GetUser(user);
        }

        public async Task<User> FindByProviderAsync(string provider, string providerUserId)
        {
            var user = await _userManager.FindByLoginAsync(provider, providerUserId);
            return GetUser(user);
        }

        public async Task<User> FindByUserId(string userId)
        {
            var userDb = await _userManager.FindByIdAsync(userId);
            return GetUser(userDb);
        }

        public async Task UpdateUserAsync(User user)
        {
            var userDb = await _userManager.FindByNameAsync(user.UserName);
            userDb.Email = user.Email;
            userDb.EmailConfirmed = user.EmailConfirmed;
            userDb.AccessFailedCount = user.AccessFailedCount;
            userDb.LockoutEnabled = user.LockoutEnabled;
            userDb.LockoutEnd = user.LockoutEnd;
            userDb.Name = user.Name;
            userDb.TwoFactorEnabled = user.TwoFactorEnabled;
            userDb.PhoneNumber = user.PhoneNumber;
            userDb.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            await _userManager.UpdateAsync(userDb);
        }

        public async Task<IEnumerable<Claim>> GetClaimByName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var claims = await _userManager.GetClaimsAsync(user);

            return claims;
        }

        public async Task<bool> SaveClaim(string userDbId, Claim claim)
        {
            var user = await _userManager.FindByIdAsync(userDbId);
            var result = await _userManager.AddClaimAsync(user, claim);

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return result.Succeeded;
        }

        public async Task<bool> RemoveClaim(string userId, string claimType, string value)
        {
            var user = await _userManager.FindByIdAsync(userId);
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

        public async Task<bool> RemoveRole(string userDbId, string requestRole)
        {
            var user = await _userManager.FindByIdAsync(userDbId);
            var result = await _userManager.RemoveFromRoleAsync(user, requestRole);

            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return result.Succeeded;
        }


        public async Task<bool> SaveRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
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

        public async Task<bool> RemoveLogin(string userId, string loginProvider, string providerKey)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            foreach (var error in result.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(result.ToString(), error.Description));
            }

            return result.Succeeded;
        }

        public async Task<IEnumerable<User>> GetUserFromRole(string role)
        {
            return (await _userManager.GetUsersInRoleAsync(role)).Select(GetUser);
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
                if (!user.EmailConfirmed)
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }
                _logger.LogInformation("Password reseted successfull.");
                return user.Id;
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


        public Task<int> Count(string search)
        {
            return search.IsPresent() ? _userManager.Users.Where(UserFind(search)).CountAsync() : _userManager.Users.CountAsync();
        }

        public async Task<string> AddLoginAsync(string email, string provider, string providerId)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            await AddLoginAsync(user, provider, providerId);

            return user.Id;
        }


        public async Task<User> FindByUsernameOrEmail(string emailOrUsername)
        {
            var user = await GetUserByEmailOrUsername(emailOrUsername);
            return GetUser(user);
        }

        private async Task<UserIdentity> GetUserByEmailOrUsername(string emailOrUsername)
        {
            UserIdentity user;
            if (emailOrUsername.IsEmail())
                user = await _userManager.FindByEmailAsync(emailOrUsername);
            else
                user = await _userManager.FindByNameAsync(emailOrUsername);
            return user;
        }
        public Task<int> Count(ICustomQueryable findByEmailNameUsername)
        {
            return _userManager.Users.Filter(findByEmailNameUsername).CountAsync();
        }

        public async Task<IEnumerable<User>> Search(ICustomQueryable search)
        {
            var users = await _userManager.Users.Apply(search).ToListAsync();
            return users.Select(GetUser);
        }
    }
}
