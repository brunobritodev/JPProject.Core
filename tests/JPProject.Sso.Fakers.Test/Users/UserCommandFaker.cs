using System;
using Bogus;
using Bogus.Extensions.UnitedStates;
using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Fakers.Test.Users
{
    public class UserCommandFaker
    {
        public static Faker<RegisterNewUserWithoutPassCommand> GenerateRegisterNewUserWithoutPassCommand()
        {
            return new Faker<RegisterNewUserWithoutPassCommand>().CustomInstantiator(
                f => new RegisterNewUserWithoutPassCommand(
                    f.Person.UserName,
                    f.Person.Email,
                    f.Person.FullName,
                    f.Image.PicsumUrl(),
                    f.Company.CompanyName(),
                    f.Rant.Random.AlphaNumeric(9)
                )
            );
        }

        public static Faker<RegisterNewUserCommand> GenerateRegisterNewUserCommand(string confirmPassword = null, DateTime? birthdate = null, string socialNumber = null)
        {
            var password = new Faker().Internet.Password();
            return new Faker<RegisterNewUserCommand>().CustomInstantiator(
                f => new RegisterNewUserCommand(
                    f.Person.UserName,
                    f.Person.Email,
                    f.Person.FullName,
                    f.Image.PicsumUrl(),
                    password,
                    confirmPassword ?? password,
                    birthdate ?? f.Person.DateOfBirth,
                    socialNumber ?? f.Person.Ssn()
                )
            );
        }

        public static Faker<AddLoginCommand> GenerateAddLoginCommand()
        {
            return new Faker<AddLoginCommand>().CustomInstantiator(
                f => new AddLoginCommand(
                    f.Person.Email,
                    f.Company.CompanyName(),
                    f.Rant.Random.AlphaNumeric(9)
                )
            );
        }

        public static Faker<SendResetLinkCommand> GenerateSendResetLinkCommand(string username = null, string email = null)
        {
            return new Faker<SendResetLinkCommand>().CustomInstantiator(
                f => new SendResetLinkCommand(
                    email ?? f.Person.Email,
                    f.Person.UserName
                )
            );
        }

    }
}
