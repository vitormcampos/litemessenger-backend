using Domain.Dtos.User;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class UserService(FinanceiroContext context, HashService hashService)
{
    public async Task<UserDataDto> Register(UserRegisterDto dto)
    {
        if (dto.ConfirmPassword != dto.Password)
        {
            throw new ArgumentException("Password and confirmPassword does not match");
        }

        if (await context.Users.AnyAsync(u => u.Username == dto.Username))
        {
            throw new ArgumentException("Username already exists");
        }

        var passwordHash = hashService.Hash(dto.Password);
        var user = dto.MapToUser(passwordHash);

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return new UserDataDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
        };
    }

    public async Task<UserDataDto> Login(UserLoginDto dto)
    {
        var user = await context.Users.FirstOrDefaultAsync(u =>
            u.Username == dto.Username || u.Email == dto.Username
        );
        if (user == null || !hashService.Verify(dto.Password, user.PasswordHash))
        {
            throw new ArgumentException("Invalid username or password");
        }

        return new UserDataDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
        };
    }
}
