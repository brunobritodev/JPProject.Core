using Bogus;
using IdentityServer4.Models;
using JPProject.Admin.Application.ViewModels.ApiScopeViewModels;
using System.Linq;

namespace JPProject.Admin.Fakers.Test.ApiResourceFakers
{
    public class ApiScopeFaker
    {
        public static Faker<ApiScope> GenerateScope()
        {
            return new Faker<ApiScope>()
                .RuleFor(s => s.Name, f => f.Lorem.Word())
                .RuleFor(s => s.DisplayName, f => f.Lorem.Word())
                .RuleFor(s => s.Description, f => f.Lorem.Word())
                .RuleFor(s => s.Required, f => f.Random.Bool())
                .RuleFor(s => s.Emphasize, f => f.Random.Bool())
                .RuleFor(s => s.ShowInDiscoveryDocument, f => f.Random.Bool())
                .RuleFor(s => s.UserClaims, f => f.PickRandom(IdentityHelpers.Claims, f.Random.Int(0, 3)).ToList());
        }

        public static Faker<SaveApiScopeViewModel> GenerateSaveScopeViewModer(string name)
        {
            return new Faker<SaveApiScopeViewModel>()
                .RuleFor(s => s.ResourceName, f => name ?? f.Lorem.Word())
                .RuleFor(s => s.Name, f => f.Lorem.Word())
                .RuleFor(s => s.DisplayName, f => f.Lorem.Word())
                .RuleFor(s => s.Description, f => f.Lorem.Word())
                .RuleFor(s => s.Required, f => f.Random.Bool())
                .RuleFor(s => s.Emphasize, f => f.Random.Bool())
                .RuleFor(s => s.ShowInDiscoveryDocument, f => f.Random.Bool());
        }
    }
}
