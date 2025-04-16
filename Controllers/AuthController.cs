using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using contrarian_reads_backend.Data;
using contrarian_reads_backend.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace contrarian_reads_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public AuthController(IConfiguration configuration, ApplicationDbContext context, IMemoryCache cache)
    {
        _configuration = configuration;
        _context = context;
        _cache = cache;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var emailPart = string.IsNullOrEmpty(loginDTO.Email) ? "unknown" : loginDTO.Email.ToLower();
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown_ip";
        var cacheKey = $"login_attempts_{emailPart}_{ipAddress}";

        var loginAttempts = _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
            return 0;
        });

        if (loginAttempts >= 5)
            return StatusCode(429, "Too many login attempts. Please try again later.");

        if (string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
        {
            loginAttempts++;
            _cache.Set(cacheKey, loginAttempts, TimeSpan.FromMinutes(15));
            return BadRequest("Email and password are required.");
        }

        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDTO.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
        {
            loginAttempts++;
            _cache.Set(cacheKey, loginAttempts, TimeSpan.FromMinutes(15));
            return Unauthorized("Invalid username or password.");
        }

        _cache.Remove(cacheKey);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");

        if (string.IsNullOrEmpty(jwtSecret))
            throw new Exception("JWT Secret not found in environment variables.");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new { Token = tokenHandler.WriteToken(token), UserId = user.Id });
    }
}