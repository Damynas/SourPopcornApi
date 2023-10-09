using Domain.Abstractions.Interfaces;

namespace Domain.Ratings.DataTransferObjects.Requests;

public sealed record UpdateRatingRequest(int Id, int SourPopcorns, string Comment) : IRequest;
