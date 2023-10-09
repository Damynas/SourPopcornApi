using Application.Abstractions.Messaging;
using Domain.Directors.DataTransferObjects.Requests;
using Domain.Directors.Entities;
using Domain.Shared.Paging;

namespace Application.Directors.Queries;

public sealed record GetDirectorsQuery(GetDirectorsRequest Request) : IQuery<PagedList<Director>>;
