namespace Presentation.Votes.DataTransferObjects;

public sealed record CreateMovieRatingVoteRequestBody(int CreatorId, bool IsPositive);
