using JPProject.EntityFrameworkCore.Interfaces;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Data.Interfaces;
using System.Threading.Tasks;

namespace JPProject.Sso.Infra.Data.UoW
{
    public class UnitOfWork : ISsoUnitOfWork
    {
        private readonly ISsoContext _context;
        private readonly IEventStoreContext _eventStoreContext;

        public UnitOfWork(ISsoContext context, IEventStoreContext eventStoreContext)
        {
            _context = context;
            _eventStoreContext = eventStoreContext;
        }

        public async Task<bool> Commit()
        {
            var linesModified = await _context.SaveChangesAsync();
            if (_eventStoreContext.GetType() != _context.GetType())
                await _eventStoreContext.SaveChangesAsync();
            return linesModified > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
            _eventStoreContext.Dispose();
        }
    }
}
