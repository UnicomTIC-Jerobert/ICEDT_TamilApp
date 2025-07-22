// TamilApp.Api/Controllers/AuthController.cs
using ICEDT.API.DTO.Request;
using ICEDT.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto registerDto)
    {
        var response = await _authService.RegisterAsync(registerDto);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto loginDto)
    {
        var response = await _authService.LoginAsync(loginDto);
        if (!response.IsSuccess)
        {
            return Unauthorized(response);
        }
        return Ok(response);
    }
}