using Application.Votes.Abstractions;
using Domain.Votes.DataTransferObjects.Responses;
using Domain.Votes.Entities;

namespace Infrastructure.Votes;

public class VoteMapper : IVoteMapper
{
    public VoteResponse ToResponse(Vote vote)
    {
        return new VoteResponse(vote.Id, vote.CreatedOn, vote.ModifiedOn, vote.RatingId, vote.CreatorId, vote.IsPositive);
    }

    public ICollection<VoteResponse> ToResponses(ICollection<Vote> votes)
    {
        return votes.Select(ToResponse).ToList();
    }
}
