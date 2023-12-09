using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Movies.Abstractions;
using Application.Ratings.Abstractions;
using Application.Ratings.Commands;
using Domain.Auth.Constants;
using Domain.Ratings.Entities;
using Domain.Shared;

namespace Application.Ratings.CommandHandlers;

public class UpdateMovieRatingCommandHandler(
    IMovieRepository movieRepository, IRatingRepository ratingRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateMovieRatingCommand, Rating?>
{
    public async Task<Result<Rating?>> Handle(UpdateMovieRatingCommand command, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetByIdAsync(command.Request.MovieId, cancellationToken);
        if (movie is null)
            return Result<Rating?>.Failure(null, Error.NullValue("Specified movie does not exist."));

        var rating = movie.Ratings.AsEnumerable().SingleOrDefault(x => x.Id == command.Request.RatingId);
        if (rating is null)
            return Result<Rating?>.Failure(null, Error.NullValue("Specified rating does not exist for specified movie."));

        if (command.Request.UserId != rating.CreatorId && !command.Request.Roles.Contains(Role.Moderator))
            return Result<Rating?>.Failure(null, Error.Forbidden("You are not allowed to update another user's rating."));

        rating.ModifiedOn = DateTime.UtcNow;
        rating.SourPopcorns = command.Request.SourPopcorns;
        rating.Comment = command.Request.Comment;

        ratingRepository.Update(rating);
        await unitOfWork.SaveChangesAsync(CancellationToken.None);

        return Result<Rating?>.Success(rating);
    }
}
