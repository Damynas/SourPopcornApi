namespace Presentation.Ratings.DataTransferObjects;

public sealed record CreateMovieRatingRequestBody(int CreatorId, int SourPopcorns, string Comment);
