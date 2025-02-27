namespace DMS.Application.Users.GetUserProfile;

public readonly record struct GetUserProfileQuery(string userId) : IQuery<ProfileResponse>;
