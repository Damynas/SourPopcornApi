using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Movies.Abstractions;
using Application.Users.Abstractions;
using Application.Votes.Abstractions;
using Application.Votes.Commands;
using Domain.Shared;
using Domain.Votes.Entities;

namespace Application.Votes.CommandHandlers;

public class CreateMovieRatingVoteCommandHandler(
    IUserRepository userRepository, IMovieRepository movieRepository, IVoteRepository voteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateMovieRatingVoteCommand, Vote?>
{
    public async Task<Result<Vote?>> Handle(CreateMovieRatingVoteCommand command, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetByIdAsync(command.Request.MovieId, cancellationToken);
        if (movie is null)
            return Result<Vote?>.Failure(null, Error.NullValue("Specified movie does not exist."));

        var rating = movie.Ratings.AsEnumerable().SingleOrDefault(x => x.Id == command.Request.RatingId);
        if (rating is null)
            return Result<Vote?>.Failure(null, Error.NullValue("Specified rating does not exist for specified movie."));

        var user = await userRepository.GetByIdAsync(command.Request.CreatorId, cancellationToken);
        if (user is null)
            return Result<Vote?>.Failure(null, Error.NullValue("Specified user does not exist."));

        var vote = new Vote(default, DateTime.UtcNow, DateTime.UtcNow)
        {
            RatingId = rating.Id,
            CreatorId = user.Id,
            IsPositive = command.Request.IsPositive
        };

        voteRepository.Add(vote);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Vote?>.Success(vote);
    }
}
