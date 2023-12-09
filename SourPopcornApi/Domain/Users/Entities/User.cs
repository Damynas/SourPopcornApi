using Domain.Abstractions.Abstracts;
using Domain.Ratings.Entities;
using Domain.Votes.Entities;

namespace Domain.Users.Entities;

public class User(int id, DateTime createdOn, DateTime modifiedOn, bool isDeleted = false) : Entity(id, createdOn, modifiedOn, isDeleted)
{
    public required string Username { get; set; }

    public required string PasswordHash { get; set; }

    public required string DisplayName { get; set; }

    public required List<string> Roles { get; set; }

    public required bool ForceLogin { get; set; }

    // Navigation properties
    public virtual ICollection<Rating> Ratings { get; } = new List<Rating>();

    public virtual ICollection<Vote> Votes { get; } = new List<Vote>();
}
