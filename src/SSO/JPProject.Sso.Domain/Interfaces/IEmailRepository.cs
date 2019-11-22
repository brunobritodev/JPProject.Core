using System.Threading.Tasks;
using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Domain.Models;

namespace JPProject.Sso.Domain.Interfaces
{
    public interface IEmailRepository : IRepository<Email>
    {
        Task<Email> GetByType(EmailType type);
    }
}