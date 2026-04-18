using Bogus;
using Domain.Models;

internal class UserBuilder
{
    private readonly Faker _faker;
    private string _username;
    private string password;
    private string _email;

    public UserBuilder()
    {
        _faker = new();
        _username = _faker.Name.FullName();
        _email = _faker.Internet.Email();
        password = _faker.Internet.Password();
    }

    public static UserBuilder New()
    {
        return new UserBuilder();
    }

    public UserBuilder WithUsername(string username)
    {
        _username = username;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithPasswordHash(string passwordHash)
    {
        this.password = passwordHash;
        return this;
    }

    public User Build()
    {
        return new User(_username, _email, password);
    }
}
