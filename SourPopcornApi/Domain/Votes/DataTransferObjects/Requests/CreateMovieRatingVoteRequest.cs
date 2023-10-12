using Domain.Abstractions.Interfaces;

namespace Domain.Votes.DataTransferObjects.Requests;

public sealed record CreateMovieRatingVoteRequest(int MovieId, int RatingId, int CreatorId, bool IsPositive) : IRequest;
