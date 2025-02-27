using DMS.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DMS.Infrastructure.Persistance;

public sealed class DateTrackingInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext != null)
        {
            IEnumerable<EntityEntry> modified = dbContext.ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Modified or EntityState.Added);

            foreach (EntityEntry item in modified)
            {
                if (item.Entity is not IDateTracking changedOrAddedItem)
                {
                    continue;
                }
                if (item.State == EntityState.Added)
                {
                    changedOrAddedItem.CreatedAt = DateTime.UtcNow;
                    changedOrAddedItem.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    changedOrAddedItem.UpdatedAt = DateTime.UtcNow;
                }
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
