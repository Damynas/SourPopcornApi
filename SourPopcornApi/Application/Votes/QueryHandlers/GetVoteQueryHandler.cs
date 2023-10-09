using Application.Abstractions.Messaging;
using Application.Votes.Abstractions;
using Application.Votes.Queries;
using Domain.Shared;
using Domain.Shared.Paging;
using Domain.Votes.Entities;

namespace Application.Votes.QueryHandlers;

public class GetVotesQueryHandler(IVoteRepository voteRepository) : IQueryHandler<GetVotesQuery, PagedList<Vote>>
{
    public async Task<Result<PagedList<Vote>>> Handle(GetVotesQuery query, CancellationToken cancellationToken)
    {
        var votePage = await voteRepository.GetAsync(query.Request.SearchParameters, cancellationToken);
        return Result<PagedList<Vote>>.Success(votePage);
    }
}
