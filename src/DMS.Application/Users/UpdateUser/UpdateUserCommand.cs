using DMS.Domain.Users;
using Microsoft.AspNetCore.Http;

namespace DMS.Application.Users.UpdateUser;

public readonly record struct UpdateUserCommand(
    string UserId,
    string Email,
    string PhoneNumber,
    DateTime? Dob,
    string FullName,
    string? UserName,
    Gender? Gender,
    string? Bio,
    IFormFile? Avatar) : ICommand<string>;
