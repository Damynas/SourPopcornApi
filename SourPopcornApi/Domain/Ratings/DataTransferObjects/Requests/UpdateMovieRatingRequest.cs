using Domain.Abstractions.Interfaces;

namespace Domain.Ratings.DataTransferObjects.Requests;

public sealed record UpdateMovieRatingRequest(int MovieId, int RatingId, int SourPopcorns, string Comment) : IRequest;
