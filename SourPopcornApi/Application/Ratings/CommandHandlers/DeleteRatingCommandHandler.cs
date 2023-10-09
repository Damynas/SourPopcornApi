using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Ratings.Abstractions;
using Application.Ratings.Commands;
using Application.Votes.Abstractions;
using Domain.Shared;

namespace Application.Ratings.CommandHandlers;

public class DeleteRatingCommandHandler(
    IRatingRepository ratingRepository, IVoteRepository voteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteRatingCommand>
{
    public async Task<Result> Handle(DeleteRatingCommand command, CancellationToken cancellationToken)
    {
        var rating = await ratingRepository.GetByIdAsync(command.Request.Id, cancellationToken);
        if (rating is null)
            return Result.Failure(Error.NullValue("Specified rating does not exist."));

        rating.ModifiedOn = DateTime.UtcNow;
        rating.IsDeleted = true;

        ratingRepository.Update(rating);

        rating.Votes.ToList().ForEach(vote =>
        {
            vote.ModifiedOn = DateTime.UtcNow;
            vote.IsDeleted = true;
            voteRepository.Update(vote);
        });

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
