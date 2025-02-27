namespace DMS.Application.Authorization;

public sealed record PermissionsResponse(string UserId, HashSet<string> Permissions);
