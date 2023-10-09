using Domain.Abstractions.Interfaces;

namespace Domain.Movies.DataTransferObjects.Requests;

public sealed record CreateMovieRequest(
    int DirectorId, string Description, string Country, string Language, DateTime ReleasedOn, List<string> Writers, List<string> Actors) : IRequest;
