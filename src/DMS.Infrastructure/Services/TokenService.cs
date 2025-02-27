using System.Security.Claims;
using System.Text;
using DMS.Application.Services;
using DMS.Domain.Settings;
using DMS.Domain.Users;
using DMS.Infrastructure.Persistance;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace DMS.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly DMSDbContext _context;
    private readonly JwtOptions _jwtOptions;
    private readonly ILogger _logger;

    public TokenService(DMSDbContext context, JwtOptions jwtOptions, ILogger logger)
    {
        _jwtOptions = jwtOptions;
        _logger = logger;
        _context = context;
    }

    public Task<string> GenerateAccessTokenAsync(User user)
    {
        var tokenHandler = new JsonWebTokenHandler();

        byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        var claimList = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, user.Id),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Subject = new ClaimsIdentity(claimList),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        string token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(token);
    }

    public string GenerateRefreshToken()
    {
        var tokenHandler = new JsonWebTokenHandler();

        byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        string token = tokenHandler.CreateToken(tokenDescriptor);
        return token;
    }

    public async Task<ClaimsIdentity?> GetPrincipalFromToken(string token)
    {
        byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidAudience = _jwtOptions.Audience,
            ValidIssuer = _jwtOptions.Issuer,
        };

        var tokenHandler = new JsonWebTokenHandler();

        TokenValidationResult principal = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
        if (principal.Exception is not null)
        {
            _logger.LogError("{Error}", principal.Exception.Message);
            return null;
        }

        return principal.ClaimsIdentity;
    }

    public bool ValidateTokenExpired(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var tokenHandler = new JsonWebTokenHandler();

        SecurityToken jwtToken = tokenHandler.ReadToken(token);

        if (jwtToken is null)
        {
            return false;
        }

        return jwtToken.ValidTo > DateTime.UtcNow;
    }
}
