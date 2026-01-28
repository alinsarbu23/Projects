using Xunit;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Insurance.Infrastructure.Services;
using Insurance.Infrastructure.Identity;
using Insurance.Application.DTOs.Identity;

namespace Insurance.UnitTests.Services;

public class AuthManagerTests
{
    private static Mock<UserManager<ApiUser>> MockUserManager()
    {
        var store = new Mock<IUserStore<ApiUser>>();

        return new Mock<UserManager<ApiUser>>(
            store.Object,
            null!, null!, null!, null!,
            null!, null!, null!, null!);
    }

    private static Mock<RoleManager<IdentityRole>> MockRoleManager(bool roleExists = true)
    {
        var store = new Mock<IRoleStore<IdentityRole>>();

        var manager = new Mock<RoleManager<IdentityRole>>(
            store.Object,
            null!, null!, null!, null!);

        manager.Setup(x => x.RoleExistsAsync(It.IsAny<string>()))
               .ReturnsAsync(roleExists);

        return manager;
    }

    private static Mock<IConfiguration> MockJwtConfig()
    {
        var config = new Mock<IConfiguration>();

        config.Setup(x => x["Jwt:Key"])
              .Returns("THIS_IS_A_VERY_LONG_SECRET_KEY_AT_LEAST_32_CHARS");

        config.Setup(x => x["Jwt:Issuer"])
              .Returns("Insurance.Api");

        config.Setup(x => x["Jwt:ExpiresMinutes"])
              .Returns("60");

        return config;
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrow_WhenEmailAlreadyExists()
    {
        var users = MockUserManager();
        var roles = MockRoleManager();
        var config = MockJwtConfig();

        users.Setup(x => x.FindByEmailAsync("a@a.ro"))
             .ReturnsAsync(new ApiUser());

        var manager = new AuthManager(users.Object, roles.Object, config.Object);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            manager.RegisterAsync(new RegisterDto
            {
                Email = "a@a.ro",
                Password = "Password1!"
            }));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenUserNotFound()
    {
        var users = MockUserManager();
        var roles = MockRoleManager();
        var config = MockJwtConfig();

        users.Setup(x => x.FindByEmailAsync("x@a.ro"))
             .ReturnsAsync((ApiUser?)null);

        var manager = new AuthManager(users.Object, roles.Object, config.Object);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            manager.LoginAsync(new LoginDto
            {
                Email = "x@a.ro",
                Password = "Password1!"
            }));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenPasswordInvalid()
    {
        var users = MockUserManager();
        var roles = MockRoleManager();
        var config = MockJwtConfig();

        var user = new ApiUser { Email = "a@a.ro" };

        users.Setup(x => x.FindByEmailAsync("a@a.ro"))
             .ReturnsAsync(user);

        users.Setup(x => x.CheckPasswordAsync(user, "wrong"))
             .ReturnsAsync(false);

        var manager = new AuthManager(users.Object, roles.Object, config.Object);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            manager.LoginAsync(new LoginDto
            {
                Email = "a@a.ro",
                Password = "wrong"
            }));
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsValid()
    {
        var users = MockUserManager();
        var roles = MockRoleManager();
        var config = MockJwtConfig();

        var user = new ApiUser
        {
            Id = "user-id",
            Email = "a@a.ro",
            UserName = "a@a.ro"
        };

        users.Setup(x => x.FindByEmailAsync("a@a.ro"))
             .ReturnsAsync(user);

        users.Setup(x => x.CheckPasswordAsync(user, "Password1!"))
             .ReturnsAsync(true);

        users.Setup(x => x.GetRolesAsync(user))
             .ReturnsAsync(new List<string> { Roles.Broker });

        var manager = new AuthManager(users.Object, roles.Object, config.Object);

        var result = await manager.LoginAsync(new LoginDto
        {
            Email = "a@a.ro",
            Password = "Password1!"
        });

        result.Token.Should().NotBeNullOrWhiteSpace();
    }
}
