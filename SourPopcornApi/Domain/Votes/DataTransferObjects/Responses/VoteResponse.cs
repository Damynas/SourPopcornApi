using Domain.Abstractions.Interfaces;

namespace Domain.Votes.DataTransferObjects.Responses;

public sealed record VoteResponse(int Id, DateTime CreatedOn, DateTime ModifiedOn, int RatingId, int CreatorId, bool IsPositive) : IResponse;
