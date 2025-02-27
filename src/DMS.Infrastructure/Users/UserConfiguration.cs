using DMS.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMS.Infrastructure.Users;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FullName).HasMaxLength(200);

        builder.Property(u => u.Email)
            .IsRequired(false)
            .HasMaxLength(300);

        builder.Property(u => u.PhoneNumber)
            .IsRequired(false)
            .HasMaxLength(300);

        builder.Property(u => u.Dob).IsRequired(false);

        builder.Property(u => u.Gender).IsRequired(false);

        builder.Property(u => u.Bio)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(u => u.AvatarUrl)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.Property(u => u.AvatarFileName)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.Property(u => u.Status)
            .HasDefaultValue(UserStatus.Inactive)
            .IsRequired(true);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasIndex(u => u.PhoneNumber).IsUnique();
    }
}
