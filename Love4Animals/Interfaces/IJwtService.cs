using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    DateTime AccessTokenExpiry { get; }
    Task RevokeTokenAsync(string jti, TimeSpan expiry);
    Task<bool> IsTokenRevokedAsync(string jti);
}
