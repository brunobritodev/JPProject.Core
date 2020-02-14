using JPProject.EntityFrameworkCore.Interfaces;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Data.Interfaces;
using System.Threading.Tasks;

namespace JPProject.Sso.Infra.Data.UoW
{
    public class UnitOfWork : ISsoUnitOfWork
    {
        private readonly IJpEntityFrameworkStore _context;

        public UnitOfWork(ISsoContext context)
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
