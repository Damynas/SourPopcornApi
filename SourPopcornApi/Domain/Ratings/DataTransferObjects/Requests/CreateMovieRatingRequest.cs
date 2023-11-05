using Domain.Abstractions.Interfaces;

namespace Domain.Ratings.DataTransferObjects.Requests;

public sealed record CreateMovieRatingRequest(int CreatorId, int MovieId, int SourPopcorns, string Comment) : IRequest;
