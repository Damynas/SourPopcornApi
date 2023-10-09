using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Ratings.Abstractions;
using Application.Ratings.Commands;
using Domain.Ratings.Entities;
using Domain.Shared;

namespace Application.Ratings.CommandHandlers;

public class UpdateRatingCommandHandler(IRatingRepository ratingRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateRatingCommand, Rating?>
{
    public async Task<Result<Rating?>> Handle(UpdateRatingCommand command, CancellationToken cancellationToken)
    {
        var rating = await ratingRepository.GetByIdAsync(command.Request.Id, cancellationToken);
        if (rating is null)
            return Result<Rating?>.Failure(null, Error.NullValue("Specified rating does not exist."));

        rating.ModifiedOn = DateTime.UtcNow;
        rating.SourPopcorns = command.Request.SourPopcorns;
        rating.Comment = command.Request.Comment;

        ratingRepository.Update(rating);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Rating?>.Success(rating);
    }
}
