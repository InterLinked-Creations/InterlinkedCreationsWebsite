using System.ComponentModel.DataAnnotations;

namespace InterlinkedCreations.Models;

public class LoginRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [MaxLength(80)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class UpdateAvatarRequest
{
    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Avatar { get; set; } = string.Empty;
}

public class ConfirmLoginRequest
{
    [Required]
    public int UserId { get; set; }
}
