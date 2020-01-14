using Newtonsoft.Json;

namespace JPProject.Sso.Application.ViewModels
{
    public class ProfilePictureViewModel : FileUploadViewModel
    {
        [JsonIgnore]
        public string Picture { get; set; }
    }

}
