namespace JPProject.Sso.Domain.ViewModels.User
{
    public struct Username
    {
        private readonly string _value;

        private Username(string value)
        {
            _value = value;

        }

        public static implicit operator Username(string value)
            => new Username(value);

        public override string ToString() => _value;
    }
}
