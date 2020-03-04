using Bogus;
using JPProject.Sso.Domain.Models;

namespace JPProject.Sso.Fakers.Test.Users
{
    public class UserFaker
    {
        public static Faker<User> GenerateUser(bool? confirmedEmail = null, string ssn = null)
        {
            return new Faker<User>()
                .RuleFor(u => u.Id, f => f.Random.Uuid().ToString())
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.EmailConfirmed, f => confirmedEmail ?? f.Random.Bool())
                .RuleFor(u => u.PasswordHash, f => f.Lorem.Word())
                .RuleFor(u => u.SecurityStamp, f => f.Lorem.Word())
                .RuleFor(u => u.PhoneNumber, f => f.Person.Phone)
                .RuleFor(u => u.PhoneNumberConfirmed, f => f.Random.Bool())
                .RuleFor(u => u.TwoFactorEnabled, f => f.Random.Bool())
                .RuleFor(u => u.LockoutEnabled, f => f.Random.Bool())
                .RuleFor(u => u.AccessFailedCount, f => f.Random.Int())
                .RuleFor(u => u.UserName, f => f.Person.UserName)
                .RuleFor(u => u.Picture, f => f.Image.LoremFlickrUrl())
                .RuleFor(u => u.Url, f => f.Internet.Url())
                .RuleFor(u => u.Name, f => f.Person.FullName)
                .RuleFor(u => u.Company, f => f.Company.CompanyName())
                .RuleFor(u => u.SocialNumber, f => ssn ?? f.Random.AlphaNumeric(15))
                .RuleFor(u => u.Bio, f => f.Lorem.Sentence())
                .RuleFor(u => u.JobTitle, f => f.Lorem.Word());
        }

    }
}
