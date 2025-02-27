using System.Security.Claims;
using DMS.Application.Users.GetUserProfile;
using DMS.Domain.Common;
using DMS.Infrastructure.Authentication;
using DMS.WebAPI.Extensions;
using MediatR;

namespace DMS.WebAPI.Users;

internal sealed class GetUserProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/profile", async (ClaimsPrincipal claims, ISender sender) =>
        {
            Result<ProfileResponse> result = await sender.Send(new GetUserProfileQuery(claims.GetUserId()));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetUser)
        .WithTags(Tags.Users);
    }
}
