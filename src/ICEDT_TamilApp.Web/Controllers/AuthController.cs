using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.Services.Interfaces;
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

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        if (request is null || string.IsNullOrEmpty(request.RefreshToken))
        {
            return BadRequest("Invalid client request");
        }

        var response = await _authService.RefreshTokenAsync(request.RefreshToken);

        if (!response.IsSuccess)
        {
            return Unauthorized(response);
        }

        return Ok(response);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] ForgotPasswordRequestDto forgotPasswordDto
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _authService.ForgotPasswordAsync(forgotPasswordDto);
        return Ok(response);
    }
    
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOTP(
        [FromBody] VerifyOTPRequestDto verifyOTPDto
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var response = await _authService.VerifyOTPAsync(verifyOTPDto);
        
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        
        return Ok(response);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordRequestDto resetPasswordDto
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _authService.ResetPasswordAsync(resetPasswordDto);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}
