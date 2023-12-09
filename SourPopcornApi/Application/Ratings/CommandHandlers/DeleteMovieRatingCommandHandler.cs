using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Movies.Abstractions;
using Application.Ratings.Abstractions;
using Application.Ratings.Commands;
using Application.Votes.Abstractions;
using Domain.Auth.Constants;
using Domain.Shared;

namespace Application.Ratings.CommandHandlers;

public class DeleteMovieRatingCommandHandler(
    IMovieRepository movieRepository, IRatingRepository ratingRepository, IVoteRepository voteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteMovieRatingCommand>
{
    public async Task<Result> Handle(DeleteMovieRatingCommand command, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetByIdAsync(command.Request.MovieId, cancellationToken);
        if (movie is null)
            return Result.Failure(Error.NullValue("Specified movie does not exist."));

        var rating = movie.Ratings.AsEnumerable().SingleOrDefault(x => x.Id == command.Request.RatingId);
        if (rating is null)
            return Result.Failure(Error.NullValue("Specified rating does not exist for specified movie."));

        if (command.Request.UserId != rating.CreatorId && !command.Request.Roles.Contains(Role.Moderator))
            return Result.Failure(Error.Forbidden("You are not allowed to delete another user's rating."));

        rating.ModifiedOn = DateTime.UtcNow;
        rating.IsDeleted = true;

        ratingRepository.Update(rating);

        rating.Votes.ToList().ForEach(vote =>
        {
            vote.ModifiedOn = DateTime.UtcNow;
            vote.IsDeleted = true;
            voteRepository.Update(vote);
        });

        await unitOfWork.SaveChangesAsync(CancellationToken.None);

        return Result.Success();
    }
}
