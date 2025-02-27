namespace DMS.Application.Users.GetUsers;

public readonly record struct GetUsersQuery() : IQuery<IEnumerable<UserResponse>>;
