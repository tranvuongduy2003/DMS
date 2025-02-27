using DMS.Application.Services;
using DMS.Domain.Common;
using DMS.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace DMS.Application.Users.UpdateUser;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, string>
{
    private readonly IFileService _fileService;
    private readonly UserManager<User> _userManager;

    public UpdateUserCommandHandler(IFileService fileService, UserManager<User> userManager)
    {
        _fileService = fileService;
        _userManager = userManager;
    }

    public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return Result.Failure<string>(UserErrors.NotFound(request.UserId));
        }

        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;
        user.Dob = request.Dob;
        user.FullName = request.FullName;
        user.UserName = request.UserName;
        user.Gender = request.Gender;
        user.Bio = request.Bio;

        if (request.Avatar != null)
        {
            if (!string.IsNullOrEmpty(user.AvatarFileName))
            {
                await _fileService.DeleteFileAsync(StorageContainers.Users, user.AvatarFileName);
            }
            using Stream stream = request.Avatar.OpenReadStream();
            string avatarUrl = await _fileService.UploadFileAsync(StorageContainers.Users, stream, request.Avatar.FileName, request.Avatar.ContentType);
            user.AvatarUrl = avatarUrl;
            user.AvatarFileName = request.Avatar.Name;
        }

        IdentityResult result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            IdentityError error = result.Errors.FirstOrDefault();
            if (error != null)
            {
                return Result.Failure<string>(Error.Failure(error.Code, error.Description));
            }

            return Result.Failure<string>(Error.NullValue);
        }

        return Result.Success(user.Id);
    }
}
