using JPProject.Sso.Domain.Models;
using Newtonsoft.Json;

namespace JPProject.Sso.Application.ViewModels.EmailViewModels
{
    public class EmailViewModel
    {
        public EmailType Type { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public BlindCarbonCopy Bcc { get; set; }
        [JsonIgnore]
        public string Username { get; set; }

        public Sender Sender { get; set; }
    }
}
