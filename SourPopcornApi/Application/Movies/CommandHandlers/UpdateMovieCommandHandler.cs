using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Directors.Abstractions;
using Application.Movies.Abstractions;
using Application.Movies.Commands;
using Domain.Movies.Entities;
using Domain.Shared;

namespace Application.Movies.CommandHandlers;

public class UpdateMovieCommandHandler(IDirectorRepository directorRepository, IMovieRepository movieRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateMovieCommand, Movie?>
{
    public async Task<Result<Movie?>> Handle(UpdateMovieCommand command, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetByIdAsync(command.Request.MovieId, cancellationToken);
        if (movie is null)
            return Result<Movie?>.Failure(null, Error.NullValue("Specified movie does not exist."));

        var director = await directorRepository.GetByIdAsync(command.Request.DirectorId, cancellationToken);
        if (director is null)
            return Result<Movie?>.Failure(null, Error.NullValue("Specified director does not exist."));


        movie.ModifiedOn = DateTime.UtcNow;
        movie.DirectorId = director.Id;
        movie.Title = command.Request.Title;
        movie.PosterLink = command.Request.PosterLink;
        movie.Description = command.Request.Description;
        movie.Country = command.Request.Country;
        movie.Language = command.Request.Language;
        movie.ReleasedOn = command.Request.ReleasedOn.ToUniversalTime();
        movie.Writers = command.Request.Writers;
        movie.Actors = command.Request.Actors;

        movieRepository.Update(movie);
        await unitOfWork.SaveChangesAsync(CancellationToken.None);

        return Result<Movie?>.Success(movie);
    }
}
