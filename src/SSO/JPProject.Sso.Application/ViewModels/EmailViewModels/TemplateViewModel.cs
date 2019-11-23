using System.ComponentModel.DataAnnotations;

namespace JPProject.Sso.Application.ViewModels.EmailViewModels
{
    public class TemplateViewModel
    {
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Name { get; set; }
        public string Username { get; set; }
        public string OldName { get; set; }

        public TemplateViewModel SetOldName(string oldname)
        {
            OldName = oldname;
            return this;
        }

    }
}