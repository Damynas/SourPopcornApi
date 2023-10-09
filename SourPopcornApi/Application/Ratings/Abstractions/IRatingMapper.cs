using Application.Abstractions.Data;
using Domain.Ratings.DataTransferObjects.Responses;
using Domain.Ratings.Entities;

namespace Application.Ratings.Abstractions;

public interface IRatingMapper : IMapper<Rating, RatingResponse> { }
