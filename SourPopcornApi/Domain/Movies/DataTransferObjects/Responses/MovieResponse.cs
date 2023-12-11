using Domain.Abstractions.Interfaces;
using Domain.Directors.DataTransferObjects.Responses;
using Domain.Ratings.DataTransferObjects.Responses;

namespace Domain.Movies.DataTransferObjects.Responses;

public sealed record MovieResponse(
    int Id, DateTime CreatedOn, DateTime ModifiedOn,
    string Title, string PosterLink, string Description, string Country, string Language, DateTime ReleasedOn, List<string> Writers, List<string> Actors,
    double SourPopcorns, DirectorResponse? Director, List<RatingResponse>? Ratings) : IResponse;
