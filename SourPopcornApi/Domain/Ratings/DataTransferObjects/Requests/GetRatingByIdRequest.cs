using Domain.Abstractions.Interfaces;

namespace Domain.Ratings.DataTransferObjects.Requests;

public sealed record GetRatingByIdRequest(int Id) : IRequest;
