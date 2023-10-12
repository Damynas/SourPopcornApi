using Domain.Abstractions.Interfaces;

namespace Domain.Movies.DataTransferObjects.Requests;

public sealed record DeleteMovieRequest(int MovieId) : IRequest;
