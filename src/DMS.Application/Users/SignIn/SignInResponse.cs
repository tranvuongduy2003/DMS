namespace DMS.Application.Users.SignIn;

public readonly record struct SignInResponse(string AccessToken, string RefreshToken);
