using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Data.Context;
using System.Threading.Tasks;

namespace JPProject.Sso.Infra.Data.UoW
{
    public class UnitOfWork : ISsoUnitOfWork
    {
        private readonly ApplicationSsoContext _context;

        public UnitOfWork(ApplicationSsoContext context)
        {
            _context = context;
        }

        public async Task<bool> Commit()
        {
            var linesModified = await _context.SaveChangesAsync();
            return linesModified > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
