using Domain.Dtos.User;

namespace Domain.Test.Dtos;

public class UserLoginDtoTest
{
    [Fact]
    public void ShouldHaveCorrectProperties()
    {
        var dto = new UserLoginDto { Username = "testuser", Password = "password123" };

        Assert.Equal("testuser", dto.Username);
        Assert.Equal("password123", dto.Password);
    }

    [Fact]
    public void ShouldAllowNullProperties()
    {
        var dto = new UserLoginDto();

        Assert.Null(dto.Username);
        Assert.Null(dto.Password);
    }
}
