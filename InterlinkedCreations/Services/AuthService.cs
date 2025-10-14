using InterlinkedCreations.Models;
using Microsoft.EntityFrameworkCore;

namespace InterlinkedCreations.Services;

public interface IAuthService
{
    Task<User?> AuthenticateAsync(string username, string password);
    Task<(bool success, List<string> errors, User? user)> RegisterAsync(string username, string email, string password);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    Task<User?> GetUserByIdAsync(int userId);
    Task<bool> UpdateAvatarAsync(int userId, string avatar);
}

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<User?> AuthenticateAsync(string usernameOrEmail, string password)
    {
        // Find user by username or email
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == usernameOrEmail || u.Email == usernameOrEmail);

        if (user == null)
            return null;

        // Verify password
        if (!_passwordHasher.VerifyPassword(password, user.Password))
            return null;

        return user;
    }

    public async Task<(bool success, List<string> errors, User? user)> RegisterAsync(string username, string email, string password)
    {
        var errors = new List<string>();

        // Validate username
        if (await UsernameExistsAsync(username))
        {
            errors.Add("Username is already taken");
        }

        // Validate email
        if (await EmailExistsAsync(email))
        {
            errors.Add("Email is already registered");
        }

        if (errors.Any())
        {
            return (false, errors, null);
        }

        // Create new user
        var user = new User
        {
            UserName = username,
            Email = email,
            Password = _passwordHasher.HashPassword(password),
            Avatar = "/lib/avatars/Colors/FillBlack.png"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return (true, new List<string>(), user);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.UserName == username);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<bool> UpdateAvatarAsync(int userId, string avatar)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return false;

        user.Avatar = avatar;
        await _context.SaveChangesAsync();
        return true;
    }
}
