namespace DMS.Domain.Common.Entities;

public interface IAuditable
{
    Guid AuthorId { get; set; }
}
