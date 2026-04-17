namespace Domain.Dtos.User;

public class UserRegisterDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

    public Models.User MapToUser(string passwordHash)
    {
        return new Models.User(Username, Email, passwordHash);
    }
}
