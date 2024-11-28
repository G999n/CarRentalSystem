using Microsoft.AspNetCore.Mvc;
using CarRentalSystem.Models;
using CarRentalSystem.Services;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // POST /users/register
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Returns validation errors
        }

        try
        {
            var newUser = await _userService.RegisterUserAsync(user);
            return Ok(new { message = "User registered successfully", user = newUser });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // POST /users/login
    [HttpPost("login")]
    public async Task<IActionResult> AuthenticateUser([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Returns validation errors
        }

        try
        {
            var token = await _userService.AuthenticateUserAsync(loginRequest.Email, loginRequest.Password);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}

// DTO for login request
public class LoginRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
