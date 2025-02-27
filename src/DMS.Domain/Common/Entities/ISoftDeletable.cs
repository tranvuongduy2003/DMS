﻿namespace DMS.Domain.Common.Entities;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    
    DateTime? DeletedAt { get; set; }
}
