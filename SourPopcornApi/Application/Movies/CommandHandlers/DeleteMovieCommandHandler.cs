using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Movies.Abstractions;
using Application.Movies.Commands;
using Application.Ratings.Abstractions;
using Application.Votes.Abstractions;
using Domain.Shared;

namespace Application.Movies.CommandHandlers;

public class DeleteMovieCommandHandler(
    IMovieRepository movieRepository, IRatingRepository ratingRepository, IVoteRepository voteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteMovieCommand>
{
    public async Task<Result> Handle(DeleteMovieCommand command, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetByIdAsync(command.Request.MovieId, cancellationToken);
        if (movie is null)
            return Result.Failure(Error.NullValue("Specified movie does not exist."));

        movie.ModifiedOn = DateTime.UtcNow;
        movie.IsDeleted = true;

        movieRepository.Update(movie);

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

        await unitOfWork.SaveChangesAsync(CancellationToken.None);

        return Result.Success();
    }
}
