using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;

namespace JPProject.Sso.Domain.ViewModels.User
{
    public class UserFindByEmailNameUsername : IQuerySort, IQueryPaging
    {
        public UserFindByEmailNameUsername(string query)
        {
            _userNameEmail = query;
        }
        private string _userNameEmail;
        [QueryOperator(Operator = WhereOperator.Contains, UseOr = true)]
        public string Email => _userNameEmail;

        [QueryOperator(Operator = WhereOperator.Contains, UseOr = true)]
        public string UserName => _userNameEmail;

        [QueryOperator(Operator = WhereOperator.Contains, UseOr = true)]
        public string Name => _userNameEmail;

        public string Sort { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}
