using Domain.Abstractions.Interfaces;
using Domain.Shared.Paging;

namespace Domain.Ratings.DataTransferObjects.Requests;

public sealed record GetRatingsRequest(SearchParameters SearchParameters) : IRequest;
