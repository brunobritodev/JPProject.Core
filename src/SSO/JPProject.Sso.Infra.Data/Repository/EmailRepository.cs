using JPProject.EntityFrameworkCore.Interfaces;
using JPProject.EntityFrameworkCore.Repository;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JPProject.Sso.Infra.Data.Repository
{
    public class EmailRepository : Repository<Email>, IEmailRepository
    {
        public EmailRepository(IJpEntityFrameworkStore context) : base(context)
        {
        }

        public Task<Email> GetByType(EmailType type)
        {
            return DbSet.FirstOrDefaultAsync(f => f.Type == type);
        }
    }
}