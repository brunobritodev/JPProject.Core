using System.Threading.Tasks;

namespace JPProject.Sso.Infra.Identity.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
