using Domain.Abstractions.Interfaces;

namespace Domain.Votes.DataTransferObjects.Requests;

public sealed record DeleteMovieRatingVoteRequest(int UserId, IEnumerable<string> Roles, int MovieId, int RatingId, int VoteId) : IRequest;
