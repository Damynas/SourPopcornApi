using Domain.Abstractions.Interfaces;

namespace Domain.Movies.DataTransferObjects.Requests;

public sealed record GetMovieByIdRequest(int Id) : IRequest;
