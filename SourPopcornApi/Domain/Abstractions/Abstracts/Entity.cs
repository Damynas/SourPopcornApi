using Domain.Abstractions.Interfaces;

namespace Domain.Abstractions.Abstracts;

public abstract class Entity : IEntity
{
    public int Id { get; init; }

    public DateTime CreatedOn { get; init; }

    public DateTime ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    protected Entity(int id, DateTime createdOn, DateTime modifiedOn, bool isDeleted)
    {
        Id = id;
        CreatedOn = createdOn;
        ModifiedOn = modifiedOn;
        IsDeleted = isDeleted;
    }
}

