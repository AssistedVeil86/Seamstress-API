using System;

namespace Seamstress.VSA.Domain.Entities;

public abstract class BaseEntity
{
    public BaseEntity()
    {
        CreatedAt = DateTimeOffset.UtcNow;
    }
    public int Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public void UpdateModified()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
