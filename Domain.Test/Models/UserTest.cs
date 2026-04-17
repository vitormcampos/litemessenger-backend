using Domain.Models;

namespace Domain.Test.Models;

public class UserTest
{
    private readonly User _user;

    public UserTest()
    {
        _user = new User("testuser", "testuser@example.com", "hashedpassword");
    }

    [Fact]
    public void ShouldBeCreated()
    {
        // Arrange
        var userName = "testuser";
        var email = "testuser@example.com";
        var passwordHash = "hashedpassword";

        // Act
        var user = new User(userName, email, passwordHash);

        // Assert
        Assert.Equal("testuser", user.Username);
        Assert.Equal("testuser@example.com", user.Email);
        Assert.Equal("hashedpassword", user.PasswordHash);
    }

    [Fact]
    public void ShouldHaveUniqueId()
    {
        // Arrange
        var user2 = new User("user2", "user2@example.com", "hashedpassword2");

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
        // Arrange
        var email = "testuser@example.com";
        var passwordHash = "hashedpassword";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new User(username, email, passwordHash));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldNotAllowNullOrEmptyEmail(string? email)
    {
        // Arrange
        var username = "testuser";
        var passwordHash = "hashedpassword";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new User(username, email, passwordHash));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldNotAllowNullOrEmptyPasswordHash(string? passwordHash)
    {
        // Arrange
        var username = "testuser";
        var email = "testuser@example.com";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new User(username, email, passwordHash));
    }

    [Fact]
    public void UpdateUsernameShouldThrowForNullOrEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _user.UpdateUsername(null!));
        Assert.Throws<ArgumentException>(() => _user.UpdateUsername(""));
    }

    [Fact]
    public void UpdateEmailShouldThrowForNullOrEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _user.UpdateEmail(null!));
        Assert.Throws<ArgumentException>(() => _user.UpdateEmail(""));
    }

    [Fact]
    public void UpdatePasswordHashShouldThrowForNullOrEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _user.UpdatePasswordHash(null!));
        Assert.Throws<ArgumentException>(() => _user.UpdatePasswordHash(""));
    }
}
