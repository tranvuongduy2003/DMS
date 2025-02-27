using DMS.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMS.Infrastructure.Users;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Name);

        builder.Property(r => r.Name).HasMaxLength(50);

        builder.Property(r => r.Description)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.HasIndex(r => r.Name).IsUnique();

        builder.HasData(
            Role.Administrator,
            Role.Accountant,
            Role.Consultant,
            Role.Teacher,
            Role.Staff);
    }
}
