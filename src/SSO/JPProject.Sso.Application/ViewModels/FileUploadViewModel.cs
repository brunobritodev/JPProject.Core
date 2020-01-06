using System.ComponentModel.DataAnnotations;

namespace JPProject.Sso.Application.ViewModels
{
    public class FileUploadViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Invalid file")]
        public string Filename { get; set; }
        [Required(ErrorMessage = "Invalid file")]
        public string FileType { get; set; }
        [Required(ErrorMessage = "Invalid file")]
        public string Value { get; set; }

        public string VirtualLocation { get; set; }
    }
}
