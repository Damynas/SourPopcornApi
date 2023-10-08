using Application.Abstractions.Messaging;
using Domain.Shared.Paging;
using Domain.Users.DataTransferObjects.Requests;
using Domain.Users.Entities;

namespace Application.Users.Queries;

public sealed record GetUsersQuery(GetUsersRequest Request) : IQuery<PagedList<User>>;
