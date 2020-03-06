using JPProject.Domain.Core.Util;
using JPProject.Sso.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;

namespace JPProject.Sso.Application.ViewModels.UserViewModels
{
    public class UserViewModel
    {
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Telephone")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Picture")]
        public string Picture { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public DateTime? Birthdate { get; set; }

        [Display(Name = "Provider")]
        public string Provider { get; set; }

        [Display(Name = "ProviderId")]
        public string ProviderId { get; set; }

        public string Url { get; set; }
        public string Company { get; set; }
        public string Bio { get; set; }
        public string JobTitle { get; set; }
        public string SocialNumber { get; set; }
        public bool ContainsFederationGateway()
        {
            return Provider.IsPresent() && ProviderId.IsPresent();
        }

        public void UpdateMetadata(List<Claim> claims)
        {
            Url = claims.ValueOf(JwtClaimTypes.WebSite);
            Company = claims.ValueOf("company");
            Bio = claims.ValueOf("bio");

            if (claims.Contains(JwtClaimTypes.BirthDate))
                Birthdate = DateTime.Parse(claims.ValueOf(JwtClaimTypes.BirthDate));

            JobTitle = claims.ValueOf("job_title");
            SocialNumber = claims.ValueOf("social_number");
            Picture = claims.ValueOf(JwtClaimTypes.Picture);
            Name = claims.ValueOf(JwtClaimTypes.GivenName);
        }
    }

}