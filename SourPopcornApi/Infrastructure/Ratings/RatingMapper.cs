using Application.Ratings.Abstractions;
using Application.Votes.Abstractions;
using Domain.Ratings.DataTransferObjects.Responses;
using Domain.Ratings.Entities;

namespace Infrastructure.Ratings;

public class RatingMapper(IVoteMapper voteMapper) : IRatingMapper
{
    private readonly IVoteMapper _voteMapper = voteMapper;

    public RatingResponse ToResponse(Rating rating)
    {
        return new RatingResponse(rating.Id, rating.CreatedOn, rating.ModifiedOn, rating.MovieId, rating.CreatorId, rating.SourPopcorns, rating.Comment,
             rating.Votes is not null ? _voteMapper.ToResponses(rating.Votes).ToList() : null);
    }

    public IEnumerable<RatingResponse> ToResponses(IEnumerable<Rating> ratings)
    {
        return ratings.Select(ToResponse).ToList();
    }
}
