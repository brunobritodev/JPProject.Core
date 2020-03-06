using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using JPProject.Sso.Domain.Interfaces;

namespace JPProject.Sso.Domain.ViewModels.User
{
    public class UserFindByEmailNameUsername : IUserSearch
    {
        public UserFindByEmailNameUsername(string query)
        {
            _userNameEmail = query;
        }
        private readonly string _userNameEmail;
        [QueryOperator(Operator = WhereOperator.Contains, UseOr = true)]
        public string Email => _userNameEmail;

        [QueryOperator(Operator = WhereOperator.Contains, UseOr = true)]
        public string UserName => _userNameEmail;



        public string Sort { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}
