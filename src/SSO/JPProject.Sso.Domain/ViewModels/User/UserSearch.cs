using JPProject.Sso.Domain.Interfaces;

namespace JPProject.Sso.Domain.ViewModels.User
{
    public class UserSearch<TKey> : IUserSearch
    {
        public TKey[] Id { get; set; }
        public string Sort { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}