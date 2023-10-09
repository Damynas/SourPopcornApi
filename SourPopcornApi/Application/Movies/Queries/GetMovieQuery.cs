using Application.Abstractions.Messaging;
using Domain.Movies.DataTransferObjects.Requests;
using Domain.Movies.Entities;
using Domain.Shared.Paging;

namespace Application.Movies.Queries;

public sealed record GetMoviesQuery(GetMoviesRequest Request) : IQuery<PagedList<Movie>>;
