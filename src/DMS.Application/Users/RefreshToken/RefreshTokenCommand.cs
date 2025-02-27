namespace DMS.Application.Users.RefreshToken;

public readonly record struct RefreshTokenCommand(string RefreshToken, string AccessToken) : ICommand<SignInResponse>;
