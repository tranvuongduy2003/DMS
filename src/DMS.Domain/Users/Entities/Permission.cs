using DMS.Domain.Common.Entities;

namespace DMS.Domain.Users.Entities;

public partial class Permission : EntityBase
{
    private Permission()
    {
    }

    private Permission(string code) : this()
    {
        Code = code;
    }

    private Permission(string code, string description) : this()
    {
        Code = code;
        Description = description;
    }

    public string Code { get; set; }

    public string? Description { get; set; }
}

public partial class Permission
{
    public static readonly Permission GetUser = new("users:read");
    public static readonly Permission CreateUser = new("users:create");
    public static readonly Permission UpdateUser = new("users:update");
    public static readonly Permission DeleteUser = new("users:delete");
}
