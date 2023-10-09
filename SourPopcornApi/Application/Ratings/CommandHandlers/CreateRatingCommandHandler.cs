using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Movies.Abstractions;
using Application.Ratings.Abstractions;
using Application.Ratings.Commands;
using Application.Users.Abstractions;
using Domain.Ratings.Entities;
using Domain.Shared;

namespace Application.Ratings.CommandHandlers;

public class CreateRatingCommandHandler(
    IUserRepository userRepository, IMovieRepository movieRepository, IRatingRepository ratingRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateRatingCommand, Rating?>
{
    public async Task<Result<Rating?>> Handle(CreateRatingCommand command, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetByIdAsync(command.Request.MovieId, cancellationToken);
        if (movie is null)
            return Result<Rating?>.Failure(null, Error.NullValue("Specified movie does not exist."));

        var user = await userRepository.GetByIdAsync(command.Request.CreatorId, cancellationToken);
        if (user is null)
            return Result<Rating?>.Failure(null, Error.NullValue("Specified user does not exist."));

        var rating = new Rating(default, DateTime.UtcNow, DateTime.UtcNow)
        {
            MovieId = movie.Id,
            CreatorId = user.Id,
            SourPopcorns = command.Request.SourPopcorns,
            Comment = command.Request.Comment
        };

        ratingRepository.Add(rating);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Rating?>.Success(rating);
    }
}
