namespace DMS.Application.Users.ChangePassword;

public readonly record struct ChangePasswordCommand(string UserId, string OldPassword, string NewPassword) : ICommand<bool>;
