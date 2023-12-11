using Domain.Abstractions.Interfaces;
using Domain.Votes.DataTransferObjects.Responses;

namespace Domain.Ratings.DataTransferObjects.Responses;

public sealed record RatingResponse(
    int Id, DateTime CreatedOn, DateTime ModifiedOn, int MovieId, int CreatorId, int SourPopcorns, string Comment, List<VoteResponse>? Votes) : IResponse;
