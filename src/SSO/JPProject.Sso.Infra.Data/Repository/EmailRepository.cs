using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JPProject.Sso.Infra.Data.Repository
{
    public class EmailRepository : Repository<Email>, IEmailRepository
    {
        public EmailRepository(ApplicationSsoContext context) : base(context)
        {
        }

        public Task<Email> GetByType(EmailType type)
        {
            return DbSet.FirstOrDefaultAsync(f => f.Type == type);
        }
    }
}