using Application.Abstractions.Messaging;
using Application.Movies.Abstractions;
using Application.Movies.Queries;
using Domain.Movies.Entities;
using Domain.Shared;
using Domain.Shared.Paging;

namespace Application.Movies.QueryHandlers;

public class GetMoviesQueryHandler(IMovieRepository movieRepository) : IQueryHandler<GetMoviesQuery, PagedList<Movie>>
{
    public async Task<Result<PagedList<Movie>>> Handle(GetMoviesQuery query, CancellationToken cancellationToken)
    {
        var moviePage = await movieRepository.GetAsync(query.Request.SearchParameters, cancellationToken);
        return Result<PagedList<Movie>>.Success(moviePage);
    }
}
