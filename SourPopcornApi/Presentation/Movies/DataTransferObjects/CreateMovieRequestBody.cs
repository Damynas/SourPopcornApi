namespace Presentation.Movies.DataTransferObjects;

public sealed record CreateMovieRequestBody(
    int DirectorId, string Title, string? PosterLink, string Description, string Country, string Language, string ReleasedOn, List<string> Writers, List<string> Actors);
