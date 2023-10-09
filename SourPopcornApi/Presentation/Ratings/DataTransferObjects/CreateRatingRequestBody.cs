namespace Presentation.Ratings.DataTransferObjects;

public sealed record CreateRatingRequestBody(int MovieId, int CreatorId, int SourPopcorns, string Comment);
