using DMS.Application.Services;
using DMS.Domain.Common;
using DMS.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DMS.Application.Users.RefreshToken;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, SignInResponse>
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public RefreshTokenCommandHandler(UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<Result<SignInResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            return Result.Failure<SignInResponse>(UserErrors.InvalidToken);
        }

        if (string.IsNullOrEmpty(request.AccessToken))
        {
            return Result.Failure<SignInResponse>(UserErrors.InvalidToken);
        }

        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        User user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Result.Failure<SignInResponse>(UserErrors.Unauthorized);
        }

        bool isValid = await _userManager.VerifyUserTokenAsync(
            user,
            TokenProviders.Default,
            TokenTypes.Refresh,
            request.RefreshToken);
        if (!isValid)
        {
            return Result.Failure<SignInResponse>(UserErrors.Unauthorized);
        }

        string newAccessToken = await _tokenService.GenerateAccessTokenAsync(user);
        string newRefreshToken =
            await _userManager.GenerateUserTokenAsync(user, TokenProviders.Default, TokenTypes.Refresh);

        var refreshResponse = new SignInResponse(newAccessToken, newRefreshToken);

        return Result.Success(refreshResponse);
    }
}
