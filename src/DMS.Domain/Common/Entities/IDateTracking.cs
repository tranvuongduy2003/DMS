namespace DMS.Domain.Common.Entities;

public interface IDateTracking
{
    DateTime CreatedAt { get; set; }

    DateTime? UpdatedAt { get; set; }
}
