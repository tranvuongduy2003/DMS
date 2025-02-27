using DMS.Domain.Users;
using Microsoft.AspNetCore.Http;

namespace DMS.Application.Users.CreateUser;

public readonly record struct CreateUserCommand(
    string Email,
    string PhoneNumber,
    DateTime? Dob,
    string FullName,
    string Password,
    Gender? Gender,
    string? Bio,
    IFormFile? Avatar) : ICommand<string>;
