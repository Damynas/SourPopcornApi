using Application.Abstractions.Messaging;
using Application.Ratings.Abstractions;
using Application.Ratings.Queries;
using Domain.Ratings.Entities;
using Domain.Shared;
using Domain.Shared.Paging;

namespace Application.Ratings.QueryHandlers;

public class GetRatingsQueryHandler(IRatingRepository ratingRepository) : IQueryHandler<GetRatingsQuery, PagedList<Rating>>
{
    public async Task<Result<PagedList<Rating>>> Handle(GetRatingsQuery query, CancellationToken cancellationToken)
    {
        var ratingPage = await ratingRepository.GetAsync(query.Request.SearchParameters, cancellationToken);
        return Result<PagedList<Rating>>.Success(ratingPage);
    }
}
