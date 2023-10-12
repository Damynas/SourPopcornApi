using Domain.Abstractions.Interfaces;

namespace Domain.Votes.DataTransferObjects.Requests;

public sealed record UpdateMovieRatingVoteRequest(int MovieId, int RatingId, int VoteId, bool IsPositive) : IRequest;
