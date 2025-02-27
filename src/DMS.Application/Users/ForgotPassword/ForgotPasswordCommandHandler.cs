using DMS.Application.Services;
using DMS.Domain.Common;
using DMS.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace DMS.Application.Users.ForgotPassword;

public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, bool>
{
    private readonly IEmailService _emailService;
    private readonly IHangfireService _hangfireService;
    private readonly UserManager<User> _userManager;

    public ForgotPasswordCommandHandler(UserManager<User> userManager,
        IHangfireService hangfireService, IEmailService emailService)
    {
        _userManager = userManager;
        _hangfireService = hangfireService;
        _emailService = emailService;
    }

    public async Task<Result<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result.Failure<bool>(UserErrors.NotFound(request.Email));
        }

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetPasswordUrl = new Uri($"{request.ResetPasswordUrl}?token={token}&email={request.Email}");

        _hangfireService.Enqueue(() =>
            _emailService
                .SendEmailAsync(request.Email, resetPasswordUrl.ToString(), "")
                .Wait());

        return Result.Success(true);
    }
}
