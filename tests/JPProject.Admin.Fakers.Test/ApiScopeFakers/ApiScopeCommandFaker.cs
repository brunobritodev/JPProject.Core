using Bogus;
using JPProject.Admin.Domain.Commands.ApiScope;

namespace JPProject.Admin.Fakers.Test.ApiScopeFakers
{
    public class ApiScopeCommandFaker
    {
        public static Faker<SaveApiScopeCommand> GenerateSaveApiScopeCommand(string name = null)
        {
            return new Faker<SaveApiScopeCommand>().CustomInstantiator(f => new SaveApiScopeCommand(
                ApiScopeFaker.GenerateApiScope().Generate()
            ));
        }

        public static Faker<UpdateApiScopeCommand> GenerateUpdateApiScopeCommand(string oldName = null, bool setName = true)
        {

            return new Faker<UpdateApiScopeCommand>().CustomInstantiator(f => new UpdateApiScopeCommand(
                    oldName ?? f.Lorem.Word(),
                    ApiScopeFaker.GenerateApiScope().Generate()));
        }

        public static Faker<RemoveApiScopeCommand> GenerateRemoveApiScopeCommand()
        {
            return new Faker<RemoveApiScopeCommand>().CustomInstantiator(faker =>
                new RemoveApiScopeCommand(faker.Random.Word())
            );

        }


    }
}