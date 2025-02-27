using DMS.Domain.Common;
using DMS.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DMS.Application.Users.GetUserProfile;

public class GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, ProfileResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public GetUserProfileQueryHandler(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Result<ProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        User user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Result.Failure<ProfileResponse>(UserErrors.NotFound(userId));
        }

        IList<string> roles = await _userManager.GetRolesAsync(user);

        return Result.Success(new ProfileResponse(
            user.Id,
            user.UserName,
            user.Email,
            user.PhoneNumber,
            user.FullName,
            user.Dob,
            user.Gender,
            user.Bio,
            user.Status,
            user.AvatarUrl,
            roles,
            user.CreatedAt,
            user.UpdatedAt));
    }
}
