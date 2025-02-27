namespace DMS.Application.Users.ResetPassword;

public readonly record struct ResetPasswordCommand(string NewPassword, string Email, string Token) : ICommand<bool>;
