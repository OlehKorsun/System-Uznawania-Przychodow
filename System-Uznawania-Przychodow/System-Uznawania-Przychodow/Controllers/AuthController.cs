using Microsoft.AspNetCore.Mvc;
using System_Uznawania_Przychodow.Requests;
using System_Uznawania_Przychodow.Services;
using LoginRequest = System_Uznawania_Przychodow.Requests.LoginRequest;

namespace System_Uznawania_Przychodow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IAuthService _authService;

    public AuthController(IConfiguration config, IAuthService authService)
    {
        _authService = authService;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto)
    {
        var res = await _authService.LoginAsync(dto);
        return Ok(res);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest dto)
    {
        await _authService.RegisterAsync(dto);
        return Ok("Rejestracja przebiegła prawidłowo!");
    }
}