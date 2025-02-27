namespace DMS.Application.Users.UpdateUserRoles;

public readonly record struct UpdateUserRolesCommand(string UserId, List<string> Roles) : ICommand<bool>;
