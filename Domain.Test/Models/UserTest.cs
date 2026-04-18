using Domain.Models;

namespace Domain.Test.Models;

public class UserTest
{
    private readonly User _user;

    public UserTest()
    {
        _user = UserBuilder.New().Build();
    }

    [Fact]
    public void ShouldBeCreated()
    {
        // Assert
        Assert.NotNull(_user);
        Assert.IsType<User>(_user);
    }

    [Fact]
    public void ShouldHaveUniqueId()
    {
        // Arrange
        var user2 = UserBuilder.New().Build();

        // Assert
        Assert.NotEqual(_user.Id, user2.Id);
    }

    [Fact]
    public void ShouldUpdateUsername()
    {
        // Arrange
        var newUsername = "updateduser";

        // Act
        _user.UpdateUsername(newUsername);

        // Assert
        Assert.Equal(newUsername, _user.Username);
    }

    [Fact]
    public void ShouldUpdateEmail()
    {
        // Arrange
        var newEmail = "updated@example.com";

        // Act
        _user.UpdateEmail(newEmail);

        // Assert
        Assert.Equal(newEmail, _user.Email);
    }

    [Fact]
    public void ShouldUpdatePasswordHash()
    {
        // Arrange
        var newPasswordHash = "updatedhashedpassword";

        // Act
        _user.UpdatePasswordHash(newPasswordHash);

        // Assert
        Assert.Equal(newPasswordHash, _user.PasswordHash);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldNotAllowNullOrEmptyUsername(string? username)
    {
        // Act
        void action() => UserBuilder.New().WithUsername(username).Build();

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldNotAllowNullOrEmptyEmail(string? email)
    {
        // Act
        void action() => UserBuilder.New().WithEmail(email).Build();

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldNotAllowNullOrEmptyPasswordHash(string? passwordHash)
    {
        // Act
        void action() => UserBuilder.New().WithPasswordHash(passwordHash).Build();

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void UpdateUsernameShouldThrowForNullOrEmpty(string? username)
    {
        // Act
        void action() => _user.UpdateUsername(username);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void UpdateEmailShouldThrowForNullOrEmpty(string? email)
    {
        // Act
        void action() => _user.UpdateEmail(email);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void UpdatePasswordHashShouldThrowForNullOrEmpty(string? passwordHash)
    {
        // Act
        void action() => _user.UpdatePasswordHash(passwordHash);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }
}
