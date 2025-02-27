namespace DMS.Application.Users.ForgotPassword;

public readonly record struct ForgotPasswordCommand(Uri ResetPasswordUrl, string Email) : ICommand<bool>;
