using Domain.Abstractions.Abstracts;
using Domain.Directors.Entities;
using Domain.Ratings.Entities;

namespace Domain.Movies.Entities;

public class Movie(int id, DateTime createdOn, DateTime modifiedOn, bool isDeleted = false) : Entity(id, createdOn, modifiedOn, isDeleted)
{
    public required int DirectorId { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required string Country { get; set; }

    public required string Language { get; set; }

    public required DateTime ReleasedOn { get; set; }

    public required List<string> Writers { get; set; }

    public required List<string> Actors { get; set; }

    // Navigation properties
    public virtual Director Director { get; set; } = null!;

    public virtual IEnumerable<Rating> Ratings { get; } = Enumerable.Empty<Rating>();
}
