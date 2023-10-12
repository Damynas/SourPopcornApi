using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Directors.Abstractions;
using Application.Movies.Abstractions;
using Application.Movies.Commands;
using Domain.Movies.Entities;
using Domain.Shared;

namespace Application.Movies.CommandHandlers;

public class CreateMovieCommandHandler(IDirectorRepository directorRepository, IMovieRepository movieRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateMovieCommand, Movie?>
{
    public async Task<Result<Movie?>> Handle(CreateMovieCommand command, CancellationToken cancellationToken)
    {
        var director = await directorRepository.GetByIdAsync(command.Request.DirectorId, cancellationToken);
        if (director is null)
            return Result<Movie?>.Failure(null, Error.NullValue("Specified director does not exist."));

        var movie = new Movie(default, DateTime.UtcNow, DateTime.UtcNow)
        {
            DirectorId = director.Id,
            Description = command.Request.Description,
            Country = command.Request.Country,
            Language = command.Request.Language,
            ReleasedOn = command.Request.ReleasedOn.ToUniversalTime(),
            Writers = command.Request.Writers,
            Actors = command.Request.Actors
        };

        movieRepository.Add(movie);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Movie?>.Success(movie);
    }
}
