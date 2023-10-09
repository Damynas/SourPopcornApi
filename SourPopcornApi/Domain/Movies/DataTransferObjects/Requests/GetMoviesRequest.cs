using Domain.Abstractions.Interfaces;
using Domain.Shared.Paging;

namespace Domain.Movies.DataTransferObjects.Requests;

public sealed record GetMoviesRequest(SearchParameters SearchParameters) : IRequest;
