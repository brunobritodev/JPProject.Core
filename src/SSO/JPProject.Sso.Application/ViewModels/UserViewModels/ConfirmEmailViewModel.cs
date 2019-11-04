using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace JPProject.Sso.Application.ViewModels.UserViewModels
{
    public class ConfirmEmailViewModel
    {
        [JsonIgnore]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }

    }
}
