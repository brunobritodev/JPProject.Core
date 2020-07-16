using System.ComponentModel.DataAnnotations;

namespace JPProject.Sso.Application.ViewModels
{
    public class RegisterUserLdapViewModel
    {
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Picture")]
        public string Picture { get; set; }
    }
}