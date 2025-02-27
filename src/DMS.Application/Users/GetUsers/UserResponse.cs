using DMS.Domain.Users;

namespace DMS.Application.Users.GetUsers;

public readonly record struct UserResponse(
    string Id,
    string? UserName,
    string? Email,
    string? PhoneNumber,
    string FullName,
    Gender? Gender,
    UserStatus Status,
    string? Avatar,
    IEnumerable<string> Roles);
