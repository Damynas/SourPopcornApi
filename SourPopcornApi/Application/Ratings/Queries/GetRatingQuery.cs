using Application.Abstractions.Messaging;
using Domain.Ratings.DataTransferObjects.Requests;
using Domain.Ratings.Entities;
using Domain.Shared.Paging;

namespace Application.Ratings.Queries;

public sealed record GetRatingsQuery(GetRatingsRequest Request) : IQuery<PagedList<Rating>>;
