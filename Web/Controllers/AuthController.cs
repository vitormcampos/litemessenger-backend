using Application.Services;
using Domain.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(JwtService jwtService, UserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto createUser)
    {
        if (createUser.ConfirmPassword != createUser.Password)
        {
            return BadRequest("Password and confirmPassword does not match");
        }

        var user = await userService.CreateUser(createUser.Username, createUser.Password);

        return Ok(new { user.Id, user.Username });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginUser)
    {
        var user = await userService.Login(loginUser.Username, loginUser.Password);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        var token = jwtService.GenerateToken(user);

        return Ok(new { token });
    }

    [HttpGet("check-auth")]
    [Authorize]
    public IActionResult CheckAuth()
    {
        return Ok(new { message = "Authenticated" });
    }
}
