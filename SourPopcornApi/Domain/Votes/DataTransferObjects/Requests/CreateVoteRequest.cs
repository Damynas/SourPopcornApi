using Domain.Abstractions.Interfaces;

namespace Domain.Votes.DataTransferObjects.Requests;

public sealed record CreateVoteRequest(int RatingId, int CreatorId, bool IsPositive) : IRequest;
