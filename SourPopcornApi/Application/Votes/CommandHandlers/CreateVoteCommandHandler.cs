using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Ratings.Abstractions;
using Application.Users.Abstractions;
using Application.Votes.Abstractions;
using Application.Votes.Commands;
using Domain.Shared;
using Domain.Votes.Entities;

namespace Application.Votes.CommandHandlers;

public class CreateVoteCommandHandler(
    IUserRepository userRepository, IRatingRepository ratingRepository, IVoteRepository voteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateVoteCommand, Vote?>
{
    public async Task<Result<Vote?>> Handle(CreateVoteCommand command, CancellationToken cancellationToken)
    {
        var rating = await ratingRepository.GetByIdAsync(command.Request.RatingId, cancellationToken);
        if (rating is null)
            return Result<Vote?>.Failure(null, Error.NullValue("Specified rating does not exist."));

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
