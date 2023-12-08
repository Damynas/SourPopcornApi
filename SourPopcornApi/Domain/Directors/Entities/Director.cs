using Domain.Abstractions.Abstracts;
using Domain.Movies.Entities;

namespace Domain.Directors.Entities;

public class Director(int id, DateTime createdOn, DateTime modifiedOn, bool isDeleted = false) : Entity(id, createdOn, modifiedOn, isDeleted)
{
    public required string Name { get; set; }

    public required string Country { get; set; }

    public required DateTime BornOn { get; set; }

    // Navigation properties
    public virtual IEnumerable<Movie> Movies { get; } = Enumerable.Empty<Movie>();
}
