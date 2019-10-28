using Bogus;
using JPProject.Sso.Application.ViewModels.UserViewModels;

namespace JPProject.Sso.Fakers.Test.Users
{
    public static class UserViewModelFaker
    {
        private static Faker _faker = new Faker();
        public static Faker<RegisterUserViewModel> GenerateUserViewModel()
        {
            var pass = _faker.Internet.Password();
            return new Faker<RegisterUserViewModel>()
                .RuleFor(r => r.Id, f => f.Random.Uuid())
                .RuleFor(r => r.Email, f => f.Person.Email)
                .RuleFor(r => r.Password, f => pass)
                .RuleFor(r => r.ConfirmPassword, f => pass)
                .RuleFor(r => r.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(r => r.Name, f => f.Person.FullName)
                .RuleFor(r => r.Username, f => f.Person.UserName)
                .RuleFor(r => r.Picture, f => f.Person.Avatar);
        }
    }
}
