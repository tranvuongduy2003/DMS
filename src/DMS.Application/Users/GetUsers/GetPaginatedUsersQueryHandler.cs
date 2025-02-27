using DMS.Domain.Common;
using DMS.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DMS.Application.Users.GetUsers;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IEnumerable<UserResponse>>
{
    private readonly UserManager<User> _userManager;

    public GetUsersQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<IEnumerable<UserResponse>>> Handle(GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        List<User> users = await _userManager.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(users.Select(x =>
        {
            IEnumerable<string> roles = _userManager.GetRolesAsync(x).GetAwaiter().GetResult();
            return new UserResponse(
                x.Id,
                x.UserName,
                x.Email,
                x.PhoneNumber,
                x.FullName,
                x.Gender,
                x.Status,
                x.AvatarUrl,
                roles);
        }));
    }
}
