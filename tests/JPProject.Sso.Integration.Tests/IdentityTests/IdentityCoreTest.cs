using FluentAssertions;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Fakers.Test.Users;
using JPProject.Sso.Infra.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JPProject.Sso.Integration.Tests.IdentityTests
{
    public class IdentityCoreTest
    {
        private IUserAppService _userAppService;

        public ApplicationIdentityContext<T> Startup<T>()
            where T : IEquatable<T>
        {
            var inMemory = new IdentityWarmup<int>(); ;
            _userAppService = inMemory.Services.GetRequiredService<IUserAppService>();
            return inMemory.Services.GetRequiredService<ApplicationIdentityContext<T>>();
        }

        [Fact]
        public async Task ShouldPrimaryKeyBeInt()
        {
            var db = Startup<int>();
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            var result = await _userAppService.Register(command);
            result.Should().BeTrue();
            db.Users.FirstOrDefault(f => f.UserName == command.Username).Should().NotBeNull();
        }
    }
}
