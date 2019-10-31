using JPProject.Sso.Application.ViewModels.UserViewModels;
using System.Collections.Generic;
using System.Linq;

namespace JPProject.Sso.Application.Extensions
{
    public static class UserListViewModelExtensions
    {
        public static UserListViewModel WithId(this IEnumerable<UserListViewModel> users, string subjectId) => users.FirstOrDefault(f => f.Id.Equals(subjectId));

    }
}
