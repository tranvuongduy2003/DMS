using DMS.Domain.Common;
using DMS.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace DMS.Application.Users.UpdateUserRoles;

public class UpdateUserRolesCommandHandler : ICommandHandler<UpdateUserRolesCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public UpdateUserRolesCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<bool>> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return Result.Failure<bool>(UserErrors.NotFound(request.UserId));
        }

        await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
        await _userManager.AddToRolesAsync(user, request.Roles);

        return Result.Success(true);
    }
}
