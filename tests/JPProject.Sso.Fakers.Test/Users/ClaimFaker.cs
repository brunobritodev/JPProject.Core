using Bogus;
using System.Security.Claims;

namespace JPProject.Sso.Fakers.Test.Users
{
    public static class ClaimFaker
    {
        public static Faker<Claim> GenerateClaim(string type = null, string value = null)
        {
            return new Faker<Claim>().CustomInstantiator(f => new Claim(f.Lorem.Word(), f.Lorem.Sentence()));
        }
    }
}
