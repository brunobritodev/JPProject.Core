using JPProject.Domain.Core.Interfaces;
using System;

namespace JPProject.AspNet.Core
{
    public class User<TKey> : IDomainUser<TKey>
        where TKey : IEquatable<TKey>

    {
        public TKey Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string Picture { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Bio { get; set; }
        public string JobTitle { get; set; }
        public IDomainUser<TKey> Create()
        {
            return new User<TKey>()
            {
                Id = Id,
                Email = Email,
                EmailConfirmed = EmailConfirmed,
                PasswordHash = PasswordHash,
                SecurityStamp = SecurityStamp,
                PhoneNumber = PhoneNumber,
                PhoneNumberConfirmed = PhoneNumberConfirmed,
                TwoFactorEnabled = TwoFactorEnabled,
                LockoutEnd = LockoutEnd,
                LockoutEnabled = LockoutEnabled,
                AccessFailedCount = AccessFailedCount,
                UserName = UserName,
                Picture = Picture,
                Url = Url,
                Name = Name,
                Company = Company,
                Bio = Bio,
                JobTitle = JobTitle,
            };
        }
    }
}
