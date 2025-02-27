using System.Security.Claims;
using DMS.Domain.Users;

namespace DMS.Application.Services;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(User user);

    string GenerateRefreshToken();

    Task<ClaimsIdentity?> GetPrincipalFromToken(string token);

    bool ValidateTokenExpired(string token);
}
