namespace Presentation.Votes.DataTransferObjects;

public sealed record CreateVoteRequestBody(int RatingId, int CreatorId, bool IsPositive);
