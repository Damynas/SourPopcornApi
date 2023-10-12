using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Directors.Abstractions;
using Application.Directors.Commands;
using Application.Movies.Abstractions;
using Application.Ratings.Abstractions;
using Application.Votes.Abstractions;
using Domain.Shared;

namespace Application.Directors.CommandHandlers;

public class DeleteDirectorCommandHandler(
    IDirectorRepository directorRepository, IMovieRepository movieRepository, IRatingRepository ratingRepository, IVoteRepository voteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteDirectorCommand>
{
    public async Task<Result> Handle(DeleteDirectorCommand command, CancellationToken cancellationToken)
    {
        var director = await directorRepository.GetByIdAsync(command.Request.DirectorId, cancellationToken);
        if (director is null)
            return Result.Failure(Error.NullValue("Specified director does not exist."));

        director.ModifiedOn = DateTime.UtcNow;
        director.IsDeleted = true;

        directorRepository.Update(director);

        director.Movies.ToList().ForEach(movie =>
        {
            movie.Ratings.ToList().ForEach(rating =>
            {
                rating.Votes.ToList().ForEach(vote =>
                {
                    vote.ModifiedOn = DateTime.UtcNow;
                    vote.IsDeleted = true;
                    voteRepository.Update(vote);
                });
                rating.ModifiedOn = DateTime.UtcNow;
                rating.IsDeleted = true;
                ratingRepository.Update(rating);
            });
            movie.ModifiedOn = DateTime.UtcNow;
            movie.IsDeleted = true;
            movieRepository.Update(movie);
        });

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
