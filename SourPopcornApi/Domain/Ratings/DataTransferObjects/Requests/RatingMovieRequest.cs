using Domain.Abstractions.Interfaces;

namespace Domain.Ratings.DataTransferObjects.Requests;

public sealed record DeleteRatingRequest(int Id) : IRequest;
