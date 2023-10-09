using Domain.Abstractions.Interfaces;

namespace Domain.Ratings.DataTransferObjects.Responses;

public sealed record RatingResponse(
    int Id, DateTime CreatedOn, DateTime ModifiedOn, int MovieId, int CreatorId, int SourPopcorns, string Comment) : IResponse;
