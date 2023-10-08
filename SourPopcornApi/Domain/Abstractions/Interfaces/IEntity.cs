namespace Domain.Abstractions.Interfaces;

public interface IEntity
{
    int Id { get; init; }

    DateTime CreatedOn { get; init; }

    DateTime ModifiedOn { get; set; }

    bool IsDeleted { get; set; }
}
