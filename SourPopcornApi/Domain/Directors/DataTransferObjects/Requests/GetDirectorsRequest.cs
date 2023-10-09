using Domain.Abstractions.Interfaces;
using Domain.Shared.Paging;

namespace Domain.Directors.DataTransferObjects.Requests;

public sealed record GetDirectorsRequest(SearchParameters SearchParameters) : IRequest;
