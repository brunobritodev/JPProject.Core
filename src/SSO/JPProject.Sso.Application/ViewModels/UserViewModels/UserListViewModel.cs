using JPProject.Domain.Core.Util;
using JPProject.Sso.Domain.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace JPProject.Sso.Application.ViewModels.UserViewModels
{
    public class UserListViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Name { get; set; }

        [Display(Name = "Picture")]
        public string Picture { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        public string Id { get; set; }

        internal void UpdateMetadata(IEnumerable<Claim> claim)
        {
            Picture = claim.ValueOf(JwtClaimTypes.Picture);
            Name = claim.ValueOf(JwtClaimTypes.GivenName);
        }
    }

}