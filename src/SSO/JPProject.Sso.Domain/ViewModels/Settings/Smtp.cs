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
        public string Server { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}