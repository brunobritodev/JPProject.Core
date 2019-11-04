using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace JPProject.Sso.Application.ViewModels
{
    public class ProfilePictureViewModel
    {
        [Required(ErrorMessage = "Invalid image")]
        public string Filename { get; set; }
        [Required(ErrorMessage = "Invalid image")]
        public string FileType { get; set; }
        [Required(ErrorMessage = "Invalid image")]
        public string Value { get; set; }

        public string Id { get; set; }
        [JsonIgnore]
        public string Picture { get; set; }
    }
}
