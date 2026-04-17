namespace Domain.Models;

public class User
{
    public string Id { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    public User(string username, string email, string passwordHash)
    {
        Id = Guid.NewGuid().ToString();

        UpdateUsername(username);
        UpdateEmail(email);
        UpdatePasswordHash(passwordHash);
    }

    public void UpdateUsername(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException("Username cannot be empty");
        }

        Username = username;
    }

    public void UpdateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Email cannot be empty");
        }

        Email = email;
    }

    public void UpdatePasswordHash(string passwordHash)
    {
        if (string.IsNullOrEmpty(passwordHash))
        {
            throw new ArgumentException("Password hash cannot be empty");
        }

        PasswordHash = passwordHash;
    }

    private User() { }
}
