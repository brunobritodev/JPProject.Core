namespace JPProject.Sso.Domain.ViewModels.User
{
    public struct AccountResult
    {
        public AccountResult(string username)
        {
            Username = username;
            Code = null;
            Url = null;
        }
        public AccountResult(string username, string code, string url)
        {
            Username = username;
            Code = code;
            Url = url;
        }
        public string Username { get; set; }
        public string Code { get; }
        public string Url { get; }
    }
}
