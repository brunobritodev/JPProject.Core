using Microsoft.AspNetCore.Identity;
using System;

namespace JPProject.Sso.Application.Configuration
{
    internal class AccountOptions
    {
        public static Action<IdentityOptions> NistAccountOptions() =>
            options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;


                // NIST Password best practices: https://pages.nist.gov/800-63-3/sp800-63b.html#appA
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
