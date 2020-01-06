namespace JPProject.Sso.Domain.ViewModels.Settings
{
    public class Smtp
    {
        public Smtp(string server, string port, string useSsl, string password, string username)
        {
            Server = server;
            Password = password;
            Username = username;

            if (int.TryParse(port, out _))
                Port = int.Parse(port);

            if (bool.TryParse(useSsl, out _))
                UseSsl = bool.Parse(useSsl);
        }
        public string Server { get; }
        public int Port { get; }
        public bool UseSsl { get; }
        public string Password { get; }
        public string Username { get; }
    }
}