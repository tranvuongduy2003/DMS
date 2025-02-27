using DMS.Domain.Common;
using DMS.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace DMS.Application.Users.ChangePassword;

internal sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public ChangePasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
        {
            return Result.Failure<bool>(UserErrors.NotFound(request.UserId));
        }

        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, request.OldPassword);

        if (!isPasswordValid)
        {
            return Result.Failure<bool>(UserErrors.WrongOldPassword);
        }

        IdentityResult result =
            await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            IdentityError error = result.Errors.FirstOrDefault();
            if (error != null)
            {
                return Result.Failure<bool>(Error.Failure(error.Code, error.Description));
            }

            return Result.Failure<bool>(Error.NullValue);
        }

        return Result.Success(true);
    }
}
