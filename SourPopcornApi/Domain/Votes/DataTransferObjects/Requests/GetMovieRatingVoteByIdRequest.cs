using Domain.Abstractions.Interfaces;

namespace Domain.Votes.DataTransferObjects.Requests;

public sealed record GetMovieRatingVoteByIdRequest(int MovieId, int RatingId, int VoteId) : IRequest;
