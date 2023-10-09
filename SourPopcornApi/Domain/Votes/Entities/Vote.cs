using Domain.Abstractions.Abstracts;
using Domain.Ratings.Entities;
using Domain.Users.Entities;

namespace Domain.Votes.Entities;

public class Vote(int id, DateTime createdOn, DateTime modifiedOn, bool isDeleted = false) : Entity(id, createdOn, modifiedOn, isDeleted)
{
    public required int RatingId { get; set; }

    public required int CreatorId { get; set; }

    public required bool IsPositive { get; set; }

    // Navigation properties

    public virtual Rating Rating { get; set; } = null!;

    public virtual User Creator { get; set; } = null!;
}
