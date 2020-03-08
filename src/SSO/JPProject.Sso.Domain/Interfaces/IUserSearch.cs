using AspNetCore.IQueryable.Extensions;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;

namespace JPProject.Sso.Domain.Interfaces
{
    public interface IUserSearch : ICustomQueryable, IQuerySort, IQueryPaging
    {
    }

}
