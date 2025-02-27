using DMS.Domain.Common;
using DMS.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace DMS.Application.Users.SignOut;

public class SignOutCommandHandler : ICommandHandler<SignOutCommand, bool>
{
    private readonly SignInManager<User> _signInManager;

    public SignOutCommandHandler(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<Result<bool>> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();

        return Result.Success(true);
    }
}
