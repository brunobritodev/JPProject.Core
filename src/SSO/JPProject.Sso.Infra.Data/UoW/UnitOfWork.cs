using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Infra.Data.Context;

namespace JPProject.Sso.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationSsoContext _context;

        public UnitOfWork(ApplicationSsoContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
