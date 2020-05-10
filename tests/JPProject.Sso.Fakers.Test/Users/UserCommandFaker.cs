using Bogus;
using Bogus.Extensions.UnitedStates;
using JPProject.Sso.Domain.Commands.User;
using JPProject.Sso.Domain.Commands.UserManagement;
using System;

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
                    f.Rant.Random.AlphaNumeric(9),
                    f.Random.Bool()
                )
            );
        }

        public static Faker<RegisterNewUserCommand> GenerateRegisterNewUserCommand(
            string confirmPassword = null,
            DateTime? birthdate = null,
            string socialNumber = null,
            bool? shouldConfirmEmail = null)
        {
            var password = new Faker().Internet.Password();
            return new Faker<RegisterNewUserCommand>().CustomInstantiator(
                f => new RegisterNewUserCommand(
                    f.Person.UserName,
                    f.Person.Email,
                    f.Person.FullName,
                    f.Person.Phone,
                    password,
                    confirmPassword ?? password,
                    birthdate ?? f.Person.DateOfBirth,
                    socialNumber ?? f.Person.Ssn(),
                    shouldConfirmEmail ?? f.Random.Bool()
                ));
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

        public static Faker<SendResetLinkCommand> GenerateSendResetLinkCommand(string username = null)
        {
            return new Faker<SendResetLinkCommand>().CustomInstantiator(
                f => new SendResetLinkCommand(username ?? f.Person.Email)
            );
        }
        public static Faker<AdminUpdateUserCommand> GenerateUpdateUserCommand()
        {
            return new Faker<AdminUpdateUserCommand>().CustomInstantiator(f => new AdminUpdateUserCommand(
                f.Person.Email,
                f.Person.UserName,
                f.Person.FullName,
                f.Person.Phone,
                f.Random.Bool(),
                f.Random.Bool(),
                f.Random.Bool(),
                f.Date.RecentOffset(),
                f.Random.Bool(),
                0,
                f.Person.DateOfBirth,
                f.Person.Ssn()));
        }
    }
}
