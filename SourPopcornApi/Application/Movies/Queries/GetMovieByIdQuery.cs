using Application.Abstractions.Messaging;
using Domain.Movies.DataTransferObjects.Requests;
using Domain.Movies.Entities;

namespace Application.Movies.Queries;

public sealed record GetMovieByIdQuery(GetMovieByIdRequest Request) : IQuery<Movie?>;
