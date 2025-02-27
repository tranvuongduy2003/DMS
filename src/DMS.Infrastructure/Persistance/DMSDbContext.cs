using DMS.Domain.Users;
using DMS.Domain.Users.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistance;

public class DMSDbContext(DbContextOptions options) : IdentityDbContext<User, Role, string>(options)
{
    public override DbSet<Role> Roles { set; get; }
    public override DbSet<User> Users { set; get; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

        base.OnModelCreating(builder);
    }
}
