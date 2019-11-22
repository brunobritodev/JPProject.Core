using System.Threading.Tasks;
using JPProject.Sso.Infra.Identity.Services;

namespace JPProject.Sso.Application.Services
{
    public class AuthSmsMessageSender : ISmsSender
    {
        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
