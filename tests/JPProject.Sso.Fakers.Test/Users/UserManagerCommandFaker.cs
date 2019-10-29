using Bogus;
using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Fakers.Test.Users
{
    public static class UserManagerCommandFaker
    {
        public static Faker<RemoveUserClaimCommand> GenerateRemoveClaimCommand(bool value = true)
        {
            return new Faker<RemoveUserClaimCommand>().CustomInstantiator(
                f => new RemoveUserClaimCommand(
                    f.Person.UserName,
                    f.Company.CompanyName(),
                    value ? f.Commerce.Department() : null
                )
            );
        }
    }
}
