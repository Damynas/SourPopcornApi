using Domain.Abstractions.Interfaces;
using Domain.Shared.Paging;

namespace Domain.Users.DataTransferObjects.Requests;

public sealed record GetUsersRequest(SearchParameters SearchParameters) : IRequest;
