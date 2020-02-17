using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;

namespace JPProject.Sso.Domain.ViewModels.User
{
    public class UserSearch : IQuerySort, IQueryPaging
    {
        [QueryOperator(Operator = WhereOperator.Equals, HasName = "SocialNumber")]
        public string Ssn { get; set; }
        public bool EmailConfirmed { get; set; }
        public string[] Id { get; set; }
        public string Sort { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}