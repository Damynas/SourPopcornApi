using Domain.Abstractions.Interfaces;

namespace Domain.Ratings.DataTransferObjects.Requests;

public sealed record DeleteMovieRatingRequest(int UserId, IEnumerable<string> Roles, int MovieId, int RatingId) : IRequest;
