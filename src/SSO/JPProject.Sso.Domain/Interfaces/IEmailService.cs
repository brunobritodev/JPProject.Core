using JPProject.Sso.Domain.ViewModels.User;
using System.Threading.Tasks;

namespace JPProject.Sso.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
