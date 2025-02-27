using DMS.Domain.Users;

namespace DMS.Application.Users.GetUserProfile;

public readonly record struct ProfileResponse(
    string Id,
    string? UserName,
    string? Email,
    string? PhoneNumber,
    string FullName,
    DateTime? Dob,
    Gender? Gender,
    string? Bio,
    UserStatus Status,
    string? Avatar,
    IEnumerable<string> Roles,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
