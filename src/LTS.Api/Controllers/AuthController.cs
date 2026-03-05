using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LTS.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LTS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserRepository userRepo, IConfiguration config) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req, CancellationToken ct)
    {
        if (await userRepo.GetByEmailAsync(req.Email, ct) != null)
            return Conflict("Email already registered");

        var user = new User
        {
            Email = req.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            Role = req.Role,
            FirstName = req.FirstName,
            LastName = req.LastName,
            Phone = req.Phone ?? string.Empty,
            Address = req.Address ?? string.Empty
        };
        await userRepo.AddAsync(user, ct);
        return Ok(new { user.Id, user.Email, user.Role, user.FirstName, user.LastName, user.Phone, Token = GenerateToken(user) });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken ct)
    {
        var user = await userRepo.GetByEmailAsync(req.Email, ct);
        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials");

        return Ok(new { user.Id, user.Email, user.Role, user.FirstName, user.LastName, user.Phone, Token = GenerateToken(user) });
    }

    string GenerateToken(User user)
    {
        var secret = config["Jwt:Secret"] ?? "lts-dev-secret-key-change-in-prod-!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("firstName", user.FirstName)
        };
        var token = new JwtSecurityToken(
            issuer: "lts-api",
            audience: "lts-client",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record RegisterRequest(string Email, string Password, UserRole Role, string FirstName, string LastName, string? Phone, string? Address);
public record LoginRequest(string Email, string Password);
