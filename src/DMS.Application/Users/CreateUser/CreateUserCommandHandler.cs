using DMS.Application.Services;
using DMS.Domain.Common;
using DMS.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace DMS.Application.Users.CreateUser;

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, string>
{
    private readonly IFileService _fileService;
    private readonly IHangfireService _hangfireService;
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;

    public CreateUserCommandHandler(IHangfireService hangfireService, IEmailService emailService, IFileService fileService, UserManager<User> userManager)
    {
        _hangfireService = hangfireService;
        _emailService = emailService;
        _fileService = fileService;
        _userManager = userManager;
    }

    public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Dob = request.Dob,
            FullName = request.FullName,
            Gender = request.Gender,
            Bio = request.Bio,
            Status = UserStatus.Active,
        };

        if (request.Avatar != null)
        {
            using Stream stream = request.Avatar.OpenReadStream();
            string avatarUrl = await _fileService.UploadFileAsync(StorageContainers.Users, stream, request.Avatar.FileName, request.Avatar.ContentType);
            user.AvatarUrl = avatarUrl;
            user.AvatarFileName = request.Avatar.Name;
        }

        IdentityResult result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            IdentityError error = result.Errors.FirstOrDefault();
            if (error != null)
            {
                return Result.Failure<string>(Error.Failure(error.Code, error.Description));
            }

            return Result.Failure<string>(Error.NullValue);
        }

        await _userManager.AddToRolesAsync(user, [UserRoles.Staff]);

        _hangfireService.Enqueue(() =>
            _emailService
                .SendEmailAsync(user.Email, user.FullName, "")
                .Wait());

        return Result.Success(user.Id);
    }
}
