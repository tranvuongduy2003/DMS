using DMS.Application.Services;
using DMS.Domain.Common;
using DMS.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace DMS.Application.Users.SignIn;

public class SignInCommandHandler : ICommandHandler<SignInCommand, SignInResponse>
{
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;

    public SignInCommandHandler(UserManager<User> userManager,
        SignInManager<User> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<Result<SignInResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        User user = _userManager.Users.FirstOrDefault(u => u.UserName == request.Username);
        if (user == null)
        {
            return Result.Failure<SignInResponse>(UserErrors.NotFound(request.Username));
        }

        bool isValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isValid)
        {
            return Result.Failure<SignInResponse>(UserErrors.WrongPassword);
        }

        if (user.Status == UserStatus.Inactive)
        {
            return Result.Failure<SignInResponse>(UserErrors.AccountDisabled);
        }

        await _signInManager.PasswordSignInAsync(user, request.Password, true, false);

        string accessToken = await _tokenService.GenerateAccessTokenAsync(user);
        string refreshToken = await _userManager.GenerateUserTokenAsync(user, TokenProviders.Default, TokenTypes.Refresh);

        await _userManager.SetAuthenticationTokenAsync(user, TokenProviders.Default, TokenTypes.Refresh, refreshToken);

        var signInResponse = new SignInResponse(accessToken, refreshToken);

        return Result.Success(signInResponse);
    }
}
