using DMS.Domain.Common.Entities;
using Microsoft.AspNetCore.Identity;

namespace DMS.Domain.Users.Entities;

public partial class Role : IdentityRole, ISoftDeletable
{
    private readonly List<Permission> _permissions = [];

    private Role()
    {
    }

    private Role(string name) : base(name)
    {
    }

    private Role(string name, string description) : base(name)
    {
        Description = description;
    }

    public string? Description { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public IReadOnlyCollection<Permission> Permissions => _permissions.ToList();
}

public partial class Role
{
    public static readonly Role Administrator = new("Administrator", "Quản trị viên");
    public static readonly Role Accountant = new("Accountant", "Kế toán");
    public static readonly Role Consultant = new("Consultant", "Tư vấn viên");
    public static readonly Role Teacher = new("Teacher", "Giáo viên");
    public static readonly Role Staff = new("Staff", "Nhân viên");
}
