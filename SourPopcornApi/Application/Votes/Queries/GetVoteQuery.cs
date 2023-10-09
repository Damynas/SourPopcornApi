using Application.Abstractions.Messaging;
using Domain.Shared.Paging;
using Domain.Votes.DataTransferObjects.Requests;
using Domain.Votes.Entities;

namespace Application.Votes.Queries;

public sealed record GetVotesQuery(GetVotesRequest Request) : IQuery<PagedList<Vote>>;
