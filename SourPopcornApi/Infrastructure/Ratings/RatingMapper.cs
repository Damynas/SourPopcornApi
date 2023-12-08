using Application.Ratings.Abstractions;
using Domain.Ratings.DataTransferObjects.Responses;
using Domain.Ratings.Entities;

namespace Infrastructure.Ratings;

public class RatingMapper : IRatingMapper
{
    public RatingResponse ToResponse(Rating rating)
    {
        return new RatingResponse(rating.Id, rating.CreatedOn, rating.ModifiedOn, rating.MovieId, rating.CreatorId, rating.SourPopcorns, rating.Comment);
    }

    public IEnumerable<RatingResponse> ToResponses(IEnumerable<Rating> ratings)
    {
        return ratings.Select(ToResponse).ToList();
    }
}
