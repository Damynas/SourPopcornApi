using Application.Abstractions.Messaging;
using Application.Votes.Abstractions;
using Application.Votes.Queries;
using Domain.Shared;
using Domain.Votes.Entities;

namespace Application.Votes.QueryHandlers;

public class GetVoteByIdQueryHandler(IVoteRepository voteRepository) : IQueryHandler<GetVoteByIdQuery, Vote?>
{
    public async Task<Result<Vote?>> Handle(GetVoteByIdQuery query, CancellationToken cancellationToken)
    {
        var vote = await voteRepository.GetByIdAsync(query.Request.Id, cancellationToken);
        if (vote is null)
            return Result<Vote?>.Failure(null, Error.NullValue("Specified vote does not exist."));

        return Result<Vote?>.Success(vote);
    }
}
