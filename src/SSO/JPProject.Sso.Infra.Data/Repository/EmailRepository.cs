using JPProject.EntityFrameworkCore.Repository;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JPProject.Sso.Infra.Data.Repository
{
    public class EmailRepository : Repository<Email>, IEmailRepository
    {
        public EmailRepository(ISsoContext context) : base(context)
        {
        }

        public Task<Email> GetByType(EmailType type)
        {
            return DbSet.FirstOrDefaultAsync(f => f.Type == type);
        }
    }
}