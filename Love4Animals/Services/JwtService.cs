using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Love4AnimalsApi.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;
    private readonly IDistributedCache _cache;

    public JwtService(IConfiguration config, IDistributedCache cache)
    {
        _config = config;
        _cache = cache;
    }

    public DateTime AccessTokenExpiry => DateTime.UtcNow.AddMinutes(20);

    public string GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.UserType.ToString()),
            new Claim(JwtRegisteredClaimNames.PhoneNumber, user.Phone.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: AccessTokenExpiry,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    public async Task RevokeTokenAsync(string jti, TimeSpan expiry)
    {
        await _cache.SetStringAsync(
            $"blacklist:{jti}",
            "revoked",
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiry }
        );
    }

    public async Task<bool> IsTokenRevokedAsync(string jti)
    {
        var value = await _cache.GetStringAsync($"blacklist:{jti}");
        return value != null;
    }
}
