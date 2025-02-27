namespace DMS.Application.Users.SignIn;

public readonly record struct SignInCommand(string Username, string Password) : ICommand<SignInResponse>;
