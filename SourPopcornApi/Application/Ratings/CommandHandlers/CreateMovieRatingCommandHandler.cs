using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Movies.Abstractions;
using Application.Ratings.Abstractions;
using Application.Ratings.Commands;
using Domain.Ratings.Entities;
using Domain.Shared;

namespace Application.Ratings.CommandHandlers;

public class CreateMovieRatingCommandHandler(
    IMovieRepository movieRepository, IRatingRepository ratingRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateMovieRatingCommand, Rating?>
{
    public async Task<Result<Rating?>> Handle(CreateMovieRatingCommand command, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.GetByIdAsync(command.Request.MovieId, cancellationToken);
        if (movie is null)
            return Result<Rating?>.Failure(null, Error.NullValue("Specified movie does not exist."));

        if (movie.Ratings.FirstOrDefault(vote => vote.CreatorId == command.Request.CreatorId) is not null)
            return Result<Rating?>.Failure(null, Error.Conflict("You cannot create a rating on the same movie more than once."));

        var rating = new Rating(default, DateTime.UtcNow, DateTime.UtcNow)
        {
            CreatorId = command.Request.CreatorId,
            MovieId = movie.Id,
            SourPopcorns = command.Request.SourPopcorns,
            Comment = command.Request.Comment
        };

        ratingRepository.Add(rating);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Rating?>.Success(rating);
    }
}
