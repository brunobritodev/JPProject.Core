using System.ComponentModel.DataAnnotations;

namespace JPProject.Sso.Application.ViewModels.UserViewModels
{
    public class ForgotPasswordViewModel
    {
        public ForgotPasswordViewModel(string username)
        {
            UsernameOrEmail = username;
        }

        [Required]
        public string UsernameOrEmail { get; set; }
    }
}
