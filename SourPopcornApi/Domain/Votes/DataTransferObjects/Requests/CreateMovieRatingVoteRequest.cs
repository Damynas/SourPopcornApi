using Domain.Abstractions.Interfaces;

namespace Domain.Votes.DataTransferObjects.Requests;

public sealed record CreateMovieRatingVoteRequest(int CreatorId, int MovieId, int RatingId, bool IsPositive) : IRequest;
