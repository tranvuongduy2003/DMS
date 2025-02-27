namespace DMS.Application.Users.RefreshToken;

public readonly record struct SignInResponse(string AccessToken, string RefreshToken);
