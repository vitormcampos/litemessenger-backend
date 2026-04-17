using Application.Services;
using Domain.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(JwtService jwtService, UserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto createUser)
    {
        var user = await userService.Register(createUser);

        return Ok(new { user.Id, user.Username });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginUser)
    {
        var user = await userService.Login(loginUser);

        var token = jwtService.GenerateToken(user);

        return Ok(new { token });
    }

    [Authorize]
    [HttpGet("check-auth")]
    public IActionResult CheckAuth()
    {
        return Ok(new { message = "Authenticated" });
    }
}
