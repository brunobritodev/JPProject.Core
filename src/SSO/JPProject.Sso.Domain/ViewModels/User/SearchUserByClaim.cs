using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using JPProject.Sso.Domain.Interfaces;

namespace JPProject.Sso.Domain.ViewModels.User
{
    public class SearchUserByClaim : IUserClaimSearch
    {
        [QueryOperator(Operator = WhereOperator.Contains, UseOr = true, HasName = "ClaimValue")]
        public string Value { get; set; }
        public string Sort { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}