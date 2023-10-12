using Domain.Abstractions.Interfaces;

namespace Domain.Ratings.DataTransferObjects.Requests;

public sealed record CreateMovieRatingRequest(int MovieId, int CreatorId, int SourPopcorns, string Comment) : IRequest;
