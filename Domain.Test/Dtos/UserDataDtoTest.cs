using Domain.Dtos.User;

namespace Domain.Test.Dtos;

public class UserDataDtoTest
{
    [Fact]
    public void ShouldHaveCorrectProperties()
    {
        var dto = new UserDataDto
        {
            Id = "123",
            Username = "testuser",
            Email = "test@example.com"
        };

        Assert.Equal("123", dto.Id);
        Assert.Equal("testuser", dto.Username);
        Assert.Equal("test@example.com", dto.Email);
    }

    [Fact]
    public void ShouldAllowNullProperties()
    {
        var dto = new UserDataDto();

        Assert.Null(dto.Id);
        Assert.Null(dto.Username);
        Assert.Null(dto.Email);
    }
}