using System.Threading.Tasks;

namespace JPProject.Sso.Infra.Identity.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
