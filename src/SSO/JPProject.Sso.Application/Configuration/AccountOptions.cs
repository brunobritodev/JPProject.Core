using Microsoft.AspNetCore.Identity;
using System;

namespace JPProject.Sso.Application.Configuration
{
    internal class AccountOptions
    {
        /// <summary>
        /// Nist account best pratices
        /// https://pages.nist.gov/800-63-3/sp800-63b.html#appA
        /// </summary>
        /// <returns></returns>
        public static Action<IdentityOptions> NistAccountOptions() =>
            options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
            };
    }
}
