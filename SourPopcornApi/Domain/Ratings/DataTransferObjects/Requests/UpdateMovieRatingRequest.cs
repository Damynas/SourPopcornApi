using Domain.Abstractions.Interfaces;

namespace Domain.Ratings.DataTransferObjects.Requests;

public sealed record UpdateMovieRatingRequest(int UserId, IEnumerable<string> Roles, int MovieId, int RatingId, int SourPopcorns, string Comment) : IRequest;
