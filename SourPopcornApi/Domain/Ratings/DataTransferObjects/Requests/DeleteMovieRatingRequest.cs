using Domain.Abstractions.Interfaces;

namespace Domain.Ratings.DataTransferObjects.Requests;

public sealed record DeleteMovieRatingRequest(int MovieId, int RatingId) : IRequest;
