using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Movies.Abstractions;
using Application.Votes.Abstractions;
using Application.Votes.Commands;
using Domain.Auth.Constants;
using Domain.Shared;

namespace Application.Votes.CommandHandlers;

public class DeleteMovieRatingVoteCommandHandler(IMovieRepository movieRepository, IVoteRepository voteRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteMovieRatingVoteCommand>
{
    public async Task<Result> Handle(DeleteMovieRatingVoteCommand command, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetByIdAsync(command.Request.MovieId, cancellationToken);
        if (movie is null)
            return Result.Failure(Error.NullValue("Specified movie does not exist."));

        var rating = movie.Ratings.AsEnumerable().SingleOrDefault(x => x.Id == command.Request.RatingId);
        if (rating is null)
            return Result.Failure(Error.NullValue("Specified rating does not exist for specified movie."));

        var vote = rating.Votes.AsEnumerable().SingleOrDefault(x => x.Id == command.Request.VoteId);
        if (vote is null)
            return Result.Failure(Error.NullValue("Specified vote does not exist for specified rating."));

        if (command.Request.UserId != rating.CreatorId && !command.Request.Roles.Contains(Role.Moderator))
            return Result.Failure(Error.Forbidden("You are not allowed to delete another user's vote."));

        vote.ModifiedOn = DateTime.UtcNow;
        vote.IsDeleted = true;

        voteRepository.Update(vote);
        await unitOfWork.SaveChangesAsync(CancellationToken.None);

        return Result.Success();
    }
}
