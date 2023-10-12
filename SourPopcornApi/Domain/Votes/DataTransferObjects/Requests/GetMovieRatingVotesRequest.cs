using Domain.Abstractions.Interfaces;
using Domain.Shared.Paging;

namespace Domain.Votes.DataTransferObjects.Requests;

public sealed record GetMovieRatingVotesRequest(int MovieId, int RatingId, SearchParameters SearchParameters) : IRequest;
