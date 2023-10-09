﻿using Domain.Abstractions.Abstracts;
using Domain.Movies.Entities;
using Domain.Users.Entities;
using Domain.Votes.Entities;

namespace Domain.Ratings.Entities;

public class Rating(int id, DateTime createdOn, DateTime modifiedOn, bool isDeleted = false) : Entity(id, createdOn, modifiedOn, isDeleted)
{
    public required int MovieId { get; set; }

    public required int CreatorId { get; set; }

    public required int SourPopcorns { get; set; }

    public required string Comment { get; set; }

    // Navigation properties

    public virtual Movie Movie { get; set; } = null!;

    public virtual User Creator { get; set; } = null!;

    public virtual ICollection<Vote> Votes { get; } = new List<Vote>();
}