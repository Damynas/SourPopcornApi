namespace Presentation.Movies.DataTransferObjects;

public sealed record UpdateMovieRequestBody(
    int DirectorId, string Description, string Country, string Language, DateTime ReleasedOn, List<string> Writers, List<string> Actors);
