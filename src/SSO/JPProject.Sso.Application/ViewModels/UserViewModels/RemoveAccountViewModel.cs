namespace JPProject.Sso.Application.ViewModels.UserViewModels
{
    public class RemoveAccountViewModel
    {
        public RemoveAccountViewModel(string username)
        {
            Username = username;
        }

        public string Username { get; set; }

    }
}