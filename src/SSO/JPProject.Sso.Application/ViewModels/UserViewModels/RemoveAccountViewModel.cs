using System;

namespace JPProject.Sso.Application.ViewModels.UserViewModels
{
    public class RemoveAccountViewModel
    {
        public RemoveAccountViewModel(Guid id)
        {
            Id = id;
        }

        public Guid? Id { get; set; }
    }
}