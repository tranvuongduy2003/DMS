using DMS.Domain.Common;
using DMS.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace DMS.Application.Users.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly UserManager<User> _userManager;

    public GetUserByIdQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound(request.UserId));
        }

        IList<string> roles = await _userManager.GetRolesAsync(user);

        return Result.Success(new UserResponse(
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
