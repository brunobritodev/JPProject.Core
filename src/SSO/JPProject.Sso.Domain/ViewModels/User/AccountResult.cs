namespace JPProject.Sso.Domain.ViewModels.User
{
    public struct AccountResult
    {
        public AccountResult(string id, string code, string url)
        {
            Id = id;
            Code = code;
            Url = url;
        }
        public string Id { get; }
        public string Code { get; }
        public string Url { get; }
    }
}
