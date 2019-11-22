namespace JPProject.Sso.Application.ViewModels.EmailViewModels
{
    public class TemplateViewModel
    {
        public string Subject { get; set; }
        public string Content { get; set; }
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