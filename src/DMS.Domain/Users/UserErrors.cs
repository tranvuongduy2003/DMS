namespace DMS.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(string userIdentifier) =>
        Error.NotFound("Users.NotFound", $"The user with the identifier {userIdentifier} was not found");

    public static readonly Error WrongPassword =
        Error.Problem("Users.WrongPassword", "Password is wrong!");

    public static readonly Error WrongOldPassword =
        Error.Problem("Users.WrongOldPassword", "Old password is wrong!");

    public static readonly Error InvalidToken =
        Error.Problem("Users.InvalidToken", "The token is invalid!");

    public static readonly Error Unauthorized = Error.Problem("Users.Unauthorized", "Unauthorized user!");

    public static readonly Error EmailAlreadyExists = Error.Conflict(
        "Users.EmailAlreadyExists",
        "A user with this email already exists");

    public static readonly Error AccountLocked = Error.Problem(
        "Users.AccountLocked",
        "This account has been locked due to multiple failed attempts");

    public static readonly Error AccountDisabled = Error.Problem(
        "Users.AccountDisabled",
        "This account has been disabled");
}
