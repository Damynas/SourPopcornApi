using Domain.Abstractions.Interfaces;

namespace Domain.Movies.DataTransferObjects.Responses;

public sealed record MovieResponse(
    int Id, DateTime CreatedOn, DateTime ModifiedOn,
    int DirectorId, string Title, string Description, string Country, string Language, DateTime ReleasedOn, List<string> Writers, List<string> Actors) : IResponse;
