namespace DMS.Application.Users.GetUserById;

public readonly record struct GetUserByIdQuery(string UserId) : IQuery<UserResponse>;
