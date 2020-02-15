using Bogus;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace JPProject.Admin.Fakers.Test.ClientFakers
{
    public class EntityClientFaker
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
        public static Faker<Claim> GenerateClientClaim()
        {
            return new Faker<Claim>()
                .RuleFor(c => c.Type, f => f.Lorem.Word())
                .RuleFor(c => c.Value, f => f.Lorem.Word());
        }

        public static Faker<Secret> GenerateClientSecret()
        {
            return new Faker<Secret>()
                .RuleFor(c => c.Description, f => f.Lorem.Word())
                .RuleFor(c => c.Value, f => f.Lorem.Word())
                .RuleFor(c => c.Type, f => f.Lorem.Word());
        }
        public static Faker<Client> GenerateClient(
            int? clientClaim = null,
            string clientId = null,
            int? clientSecrets = null,
            int? clientProperties = null)
        {
            return new Faker<Client>()
                .RuleFor(c => c.Enabled, f => f.Random.Bool())
                .RuleFor(c => c.ClientId, f => clientId ?? f.Lorem.Word())
                .RuleFor(c => c.ProtocolType, f => f.Lorem.Word())
                .RuleFor(c => c.RequireClientSecret, f => f.Random.Bool())
                .RuleFor(c => c.ClientName, f => f.Lorem.Word())
                .RuleFor(c => c.Description, f => f.Lorem.Word())
                .RuleFor(c => c.ClientUri, f => f.Lorem.Word())
                .RuleFor(c => c.LogoUri, f => f.Lorem.Word())
                .RuleFor(c => c.RequireConsent, f => f.Random.Bool())
                .RuleFor(c => c.AllowRememberConsent, f => f.Random.Bool())
                .RuleFor(c => c.AlwaysIncludeUserClaimsInIdToken, f => f.Random.Bool())
                .RuleFor(c => c.RequirePkce, f => f.Random.Bool())
                .RuleFor(c => c.AllowPlainTextPkce, f => f.Random.Bool())
                .RuleFor(c => c.AllowAccessTokensViaBrowser, f => f.Random.Bool())
                .RuleFor(c => c.FrontChannelLogoutUri, f => f.Lorem.Word())
                .RuleFor(c => c.FrontChannelLogoutSessionRequired, f => f.Random.Bool())
                .RuleFor(c => c.BackChannelLogoutUri, f => f.Lorem.Word())
                .RuleFor(c => c.BackChannelLogoutSessionRequired, f => f.Random.Bool())
                .RuleFor(c => c.AllowOfflineAccess, f => f.Random.Bool())
                .RuleFor(c => c.IdentityTokenLifetime, f => f.Random.Int())
                .RuleFor(c => c.AccessTokenLifetime, f => f.Random.Int())
                .RuleFor(c => c.AuthorizationCodeLifetime, f => f.Random.Int())
                .RuleFor(c => c.AbsoluteRefreshTokenLifetime, f => f.Random.Int())
                .RuleFor(c => c.SlidingRefreshTokenLifetime, f => f.Random.Int())
                .RuleFor(c => c.RefreshTokenUsage, f => f.PickRandom<TokenUsage>())
                .RuleFor(c => c.UpdateAccessTokenClaimsOnRefresh, f => f.Random.Bool())
                .RuleFor(c => c.RefreshTokenExpiration, f => f.PickRandom<TokenExpiration>())
                .RuleFor(c => c.AccessTokenType, f => f.PickRandom<AccessTokenType>())
                .RuleFor(c => c.EnableLocalLogin, f => f.Random.Bool())
                .RuleFor(c => c.IncludeJwtId, f => f.Random.Bool())
                .RuleFor(c => c.AlwaysSendClientClaims, f => f.Random.Bool())
                .RuleFor(c => c.ClientClaimsPrefix, f => f.Lorem.Word())
                .RuleFor(c => c.PairWiseSubjectSalt, f => f.Lorem.Word())
                .RuleFor(c => c.UserCodeType, f => f.Lorem.Word())
                .RuleFor(c => c.DeviceCodeLifetime, f => f.Random.Int())
                .RuleFor(c => c.AllowedScopes, f => Enumerable.Range(0, IdentityHelpers.Scopes.Length).Select(s => f.PickRandom(IdentityHelpers.Scopes)))
                .RuleFor(c => c.ClientSecrets, f => GenerateClientSecret().Generate(clientSecrets ?? f.Random.Int(0, 3)))
                .RuleFor(c => c.AllowedGrantTypes, f => new[] { f.PickRandom(IdentityHelpers.Grantypes) })
                .RuleFor(c => c.RedirectUris, f => Enumerable.Range(0, IdentityHelpers.Scopes.Length).Select(s => f.Internet.Url()))
                .RuleFor(c => c.PostLogoutRedirectUris, f => Enumerable.Range(0, IdentityHelpers.Scopes.Length).Select(s => f.Internet.Url()))
                .RuleFor(c => c.Claims, f => GenerateClientClaim().Generate(clientClaim ?? f.Random.Int(1, 5)))
                .RuleFor(c => c.IdentityProviderRestrictions, f => Enumerable.Range(0, IdentityHelpers.Scopes.Length).Select(s => f.PickRandom(IdentityHelpers.Providers)))
                .RuleFor(c => c.AllowedCorsOrigins, f => Enumerable.Range(0, IdentityHelpers.Scopes.Length).Select(s => f.Internet.Url()))
                .RuleFor(c => c.Properties, f => RandomData());

        }
    }
}
