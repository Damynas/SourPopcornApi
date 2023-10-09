using Domain.Abstractions.Interfaces;
using Domain.Shared.Paging;

namespace Domain.Votes.DataTransferObjects.Requests;

public sealed record GetVotesRequest(SearchParameters SearchParameters) : IRequest;
