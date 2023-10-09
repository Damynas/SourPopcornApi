using Application.Abstractions.Messaging;
using Application.Directors.Abstractions;
using Application.Directors.Queries;
using Domain.Directors.Entities;
using Domain.Shared;
using Domain.Shared.Paging;

namespace Application.Directors.QueryHandlers;

public class GetDirectorsQueryHandler(IDirectorRepository directorRepository) : IQueryHandler<GetDirectorsQuery, PagedList<Director>>
{
    public async Task<Result<PagedList<Director>>> Handle(GetDirectorsQuery query, CancellationToken cancellationToken)
    {
        var directorPage = await directorRepository.GetAsync(query.Request.SearchParameters, cancellationToken);
        return Result<PagedList<Director>>.Success(directorPage);
    }
}
