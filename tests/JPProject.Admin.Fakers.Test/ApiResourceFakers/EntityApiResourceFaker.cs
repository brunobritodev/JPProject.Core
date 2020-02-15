using Bogus;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace JPProject.Admin.Fakers.Test.ApiResourceFakers
{
    public class EntityApiResourceFaker
    {
        public static Dictionary<string, string> RandomData()
        {
            var faker = new Faker();
            var dic = new Dictionary<string, string>();
            foreach (var item in Enumerable.Range(1, faker.Random.Int(1, 10)))
            {
                var key = faker.Company.CompanyName();
                if (!dic.ContainsKey(key))
                    dic.Add(key, faker.Commerce.Product());
            }

            return dic;
        }
        public static Faker<ApiResource> GenerateResource()
        {
            return new Faker<ApiResource>()
                .RuleFor(a => a.Enabled, f => f.Random.Bool())
                .RuleFor(a => a.Name, f => f.Lorem.Word())
                .RuleFor(a => a.DisplayName, f => f.Lorem.Word())
                .RuleFor(a => a.Description, f => f.Lorem.Word())
                .RuleFor(a => a.ApiSecrets, f => GenerateSecrets().Generate(f.Random.Int(0, 3)))
                .RuleFor(a => a.Scopes, f => GenerateScopes().Generate(f.Random.Int(0, 3)))
                .RuleFor(a => a.UserClaims, f => Enumerable.Range(0, f.Random.Int(0, 10)).Select(s => f.Commerce.Product()))
                .RuleFor(a => a.Properties, f => RandomData());
        }

        public static Faker<Secret> GenerateSecrets()
        {
            return new Faker<Secret>()
                .RuleFor(a => a.Description, f => f.Lorem.Word())
                .RuleFor(a => a.Value, f => f.Lorem.Word())
                .RuleFor(a => a.Type, f => f.Lorem.Word());
        }
        public static Faker<Scope> GenerateScopes()
        {
            return new Faker<Scope>()
                .RuleFor(a => a.Name, f => f.Lorem.Word())
                .RuleFor(a => a.DisplayName, f => f.Lorem.Word())
                .RuleFor(a => a.Description, f => f.Lorem.Word())
                .RuleFor(a => a.Required, f => f.Random.Bool())
                .RuleFor(a => a.Emphasize, f => f.Random.Bool())
                .RuleFor(a => a.ShowInDiscoveryDocument, f => f.Random.Bool())
                .RuleFor(a => a.UserClaims, f => Enumerable.Range(0, f.Random.Int(0, 10)).Select(s => f.Commerce.Product()));
        }
        public static Faker<Claim> GenerateClaim()
        {
            return new Faker<Claim>()
                .RuleFor(c => c.Type, f => f.Lorem.Word())
                .RuleFor(c => c.ValueType, f => f.Lorem.Word());
        }

    }
}
