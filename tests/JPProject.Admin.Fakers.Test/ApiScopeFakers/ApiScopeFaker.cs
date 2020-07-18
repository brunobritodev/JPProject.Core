using System.Linq;
using Bogus;
using IdentityServer4.Models;

namespace JPProject.Admin.Fakers.Test.ApiScopeFakers
{
    public class ApiScopeFaker
    {
        public static Faker<ApiScope> GenerateApiScope()
        {
            return new Faker<ApiScope>()
                .RuleFor(s => s.Name, f => f.Internet.DomainWord())
                .RuleFor(s => s.DisplayName, f => f.Lorem.Word())
                .RuleFor(s => s.Description, f => f.Lorem.Word())
                .RuleFor(s => s.Required, f => f.Random.Bool())
                .RuleFor(s => s.Emphasize, f => f.Random.Bool())
                .RuleFor(s => s.ShowInDiscoveryDocument, f => f.Random.Bool())
                .RuleFor(s => s.UserClaims, f => f.PickRandom(IdentityHelpers.Claims, f.Random.Int(0, 3)).ToList());
        }
    }
}