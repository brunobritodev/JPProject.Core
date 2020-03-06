using Bogus;
using FluentAssertions;
using JPProject.Domain.Core.Util;
using Xunit;

namespace JPProject.Sso.Domain.Tests.UtilTests
{
    public class StringUtilTests
    {
        private Faker _faker;

        public StringUtilTests()
        {
            this._faker = new Faker();
        }

        [Fact]
        public void Should_Truncate_Email()
        {
            var email = _faker.Person.Email;
            var originalEmail = email;
            email = email.TruncateEmail();

            email.Should().NotMatch(originalEmail);
            email[0].Should().Be(originalEmail[0]);
            email[^1].Should().Be(originalEmail[^1]);
        }
    }
}
