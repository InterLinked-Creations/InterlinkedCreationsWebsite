using InterlinkedCreations.Models;
using InterlinkedCreations.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterlinkedCreations.Controllers;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private const string SessionKeyUserId = "UserId";
    private const string SessionKeyUsername = "Username";
    private const string SessionKeyAvatar = "Avatar";

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return Ok(new { errors });
        }

        var (success, validationErrors, user) = await _authService.RegisterAsync(
            request.Username,
            request.Email,
            request.Password
        );

        if (!success)
        {
            return Ok(new { errors = validationErrors });
        }

        // Set session
        HttpContext.Session.SetInt32(SessionKeyUserId, user!.UserID);
        HttpContext.Session.SetString(SessionKeyUsername, user.UserName);
        HttpContext.Session.SetString(SessionKeyAvatar, user.Avatar);

        return Ok(new
        {
            userId = user.UserID,
            username = user.UserName,
            avatar = user.Avatar
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(new { errors = new[] { "Invalid request" } });
        }

        var user = await _authService.AuthenticateAsync(request.Username, request.Password);

        if (user == null)
        {
            return Ok(new { errors = new[] { "Invalid username or password" } });
        }

        // Return user data for confirmation (don't set session yet)
        return Ok(new
        {
            userId = user.UserID,
            username = user.UserName,
            avatar = user.Avatar
        });
    }

    [HttpPost("confirm-login")]
    public async Task<IActionResult> ConfirmLogin([FromBody] ConfirmLoginRequest request)
    {
        var user = await _authService.GetUserByIdAsync(request.UserId);

        if (user == null)
        {
            return BadRequest(new { error = "User not found" });
        }

        // Set session
        HttpContext.Session.SetInt32(SessionKeyUserId, user.UserID);
        HttpContext.Session.SetString(SessionKeyUsername, user.UserName);
        HttpContext.Session.SetString(SessionKeyAvatar, user.Avatar);

        return Ok(new { success = true });
    }

    [HttpGet("check-session")]
    public IActionResult CheckSession()
    {
        var userId = HttpContext.Session.GetInt32(SessionKeyUserId);

        if (userId.HasValue)
        {
            var username = HttpContext.Session.GetString(SessionKeyUsername);
            var avatar = HttpContext.Session.GetString(SessionKeyAvatar);

            return Ok(new
            {
                authenticated = true,
                userId = userId.Value,
                username,
                avatar
            });
        }

        return Ok(new { authenticated = false });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok(new { success = true });
    }

    [HttpPost("update-avatar")]
    public async Task<IActionResult> UpdateAvatar([FromBody] UpdateAvatarRequest request)
    {
        var userId = HttpContext.Session.GetInt32(SessionKeyUserId);

        if (!userId.HasValue || userId.Value != request.UserId)
        {
            return Unauthorized(new { error = "Not authorized" });
        }

        var success = await _authService.UpdateAvatarAsync(request.UserId, request.Avatar);

        if (!success)
        {
            return BadRequest(new { error = "Failed to update avatar" });
        }

        // Update session
        HttpContext.Session.SetString(SessionKeyAvatar, request.Avatar);

        return Ok(new { success = true });
    }

    [HttpGet("avatars")]
    public IActionResult GetAvatars()
    {
        // List of available avatars
        var avatars = new[]
        {
            "/lib/avatars/Colors/FillBlack.png",
            "/lib/avatars/Colors/FillBlue.png",
            "/lib/avatars/Colors/FillBrown.png",
            "/lib/avatars/Colors/FillGreen.png",
            "/lib/avatars/Colors/FillOrange.png",
            "/lib/avatars/Colors/FillPink.png",
            "/lib/avatars/Colors/FillPurple.png",
            "/lib/avatars/Colors/FillRed.png",
            "/lib/avatars/Colors/FillYellow.png",
            "/lib/avatars/Colors/LineBlack.png",
            "/lib/avatars/Colors/LineBlue.png",
            "/lib/avatars/Colors/LineBrown.png",
            "/lib/avatars/Colors/LineGreen.png",
            "/lib/avatars/Colors/LineOrange.png",
            "/lib/avatars/Colors/LinePink.png",
            "/lib/avatars/Colors/LinePurple.png",
            "/lib/avatars/Colors/LineRed.png",
            "/lib/avatars/Colors/LineYellow.png"
        };

        return Ok(new { avatars });
    }
}
