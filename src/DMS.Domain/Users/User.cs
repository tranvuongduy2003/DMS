using DMS.Domain.Common.AggregateRoot;
using DMS.Domain.Common.DomainEvent;
using DMS.Domain.Common.Entities;
using DMS.Domain.Users.Entities;
using Microsoft.AspNetCore.Identity;

namespace DMS.Domain.Users;

public partial class User : IdentityUser, IAggregateRoot, IDateTracking, ISoftDeletable
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private readonly List<Role> _roles = [];

    public string FullName { get; set; }

    public DateTime? Dob { get; set; }

    public Gender? Gender { get; set; }

    public string? Bio { get; set; }

    public string? AvatarUrl { get; set; }

    public string? AvatarFileName { get; set; }

    public UserStatus Status { get; set; } = UserStatus.Active;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public IReadOnlyCollection<Role> Roles => _roles.ToList();

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
}

public partial class User
{
    public static User Create(string email, string phoneNumber, string fullName, DateTime? dob, Gender? gender, string? bio, Uri? avatarUrl, string? avatarFileName)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            PhoneNumber = phoneNumber,
            FullName = fullName,
            Dob = dob,
            Gender = gender,
            Bio = bio,
            AvatarUrl = avatarUrl?.AbsoluteUri,
            AvatarFileName = avatarFileName
        };

        user._roles.Add(Role.Staff);

        //user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id));

        return user;
    }

    public void Update(string fullName, DateTime? dob, Gender? gender, string? bio, Uri? avatarUrl, string? avatarFileName)
    {
        FullName = fullName;
        Dob = dob;
        Gender = gender;
        Bio = bio;
        AvatarUrl = avatarUrl?.AbsoluteUri;
        AvatarFileName = avatarFileName;

        //Raise(new UserProfileUpdatedDomainEvent(Id, FirstName, LastName));
    }
}
