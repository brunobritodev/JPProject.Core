using System;

namespace JPProject.Domain.Core.Interfaces
{
    /// <summary>
    /// Interface to propagate user id type
    /// </summary>
    public interface IDomainUser
    {
        string UserName { get; }
        string Email { get; }
        bool EmailConfirmed { get; }
        string PasswordHash { get; }
        string SecurityStamp { get; }
        string PhoneNumber { get; }
        bool PhoneNumberConfirmed { get; }
        bool TwoFactorEnabled { get; }
        DateTimeOffset? LockoutEnd { get; }
        bool LockoutEnabled { get; }
        int AccessFailedCount { get; }
        string Picture { get; }
        string Name { get; }
        string Url { get; }
        string Company { get; }
        string Bio { get; }
        string JobTitle { get; }
        string SocialNumber { get; }
        DateTime? Birthdate { get; }
    }

    public interface IDomainUserFactory<T>
        where T : class
    {
        T CreateUser(IDomainUser user);
    }
}