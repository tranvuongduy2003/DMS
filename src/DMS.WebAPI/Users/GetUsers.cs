using System.Security.Claims;
using DMS.Application.Users.GetUsers;
using DMS.Domain.Common;
using DMS.WebAPI.Extensions;
using MediatR;

namespace DMS.WebAPI.Users;

internal sealed class GetUsers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users", async (ClaimsPrincipal claims, ISender sender) =>
        {
            Result<IEnumerable<UserResponse>> result = await sender.Send(new GetUsersQuery());

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetUser)
        .WithTags(Tags.Users);
    }
}
