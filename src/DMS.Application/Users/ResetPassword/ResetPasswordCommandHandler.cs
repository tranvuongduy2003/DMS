using DMS.Domain.Common;
using DMS.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace DMS.Application.Users.ResetPassword;

public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result.Failure<bool>(UserErrors.NotFound(request.Email));
        }

        IdentityResult result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded)
        {
            IdentityError error = result.Errors.FirstOrDefault();
            if (error != null)
            {
                return Result.Failure<bool>(Error.Failure(error.Code, error.Description));
            }

            return Result.Failure<bool>(Error.NullValue);
        }

        return true;
    }
}
