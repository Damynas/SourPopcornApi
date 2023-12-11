using Domain.Abstractions.Interfaces;

namespace Domain.Movies.DataTransferObjects.Requests;

public sealed record UpdateMovieRequest(
    int MovieId, int DirectorId, string Title, string PosterLink, string Description, string Country, string Language, DateTime ReleasedOn, List<string> Writers, List<string> Actors) : IRequest;
