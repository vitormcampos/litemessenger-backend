using Domain.Dtos.User;

namespace Domain.Test.Dtos;

public class UserRegisterDtoTest
{
    [Fact]
    public void ShouldHaveCorrectProperties()
    {
        var dto = new UserRegisterDto
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };

        Assert.Equal("testuser", dto.Username);
        Assert.Equal("test@example.com", dto.Email);
        Assert.Equal("password123", dto.Password);
        Assert.Equal("password123", dto.ConfirmPassword);
    }

    [Fact]
    public void MapToUserShouldCreateUser()
    {
        var dto = new UserRegisterDto
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };
        var passwordHash = "hashedpassword";

        var user = dto.MapToUser(passwordHash);

        Assert.NotNull(user);
        Assert.Equal("testuser", user.Username);
        Assert.Equal("test@example.com", user.Email);
    }
}