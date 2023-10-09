namespace Presentation.Movies.DataTransferObjects;

public sealed record CreateMovieRequestBody(
    int DirectorId, string Description, string Country, string Language, DateTime ReleasedOn, List<string> Writers, List<string> Actors);
