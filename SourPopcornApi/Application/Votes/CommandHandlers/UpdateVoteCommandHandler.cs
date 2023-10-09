using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Votes.Abstractions;
using Application.Votes.Commands;
using Domain.Shared;
using Domain.Votes.Entities;

namespace Application.Votes.CommandHandlers;

public class UpdateVoteCommandHandler(IVoteRepository voteRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateVoteCommand, Vote?>
{
    public async Task<Result<Vote?>> Handle(UpdateVoteCommand command, CancellationToken cancellationToken)
    {
        var vote = await voteRepository.GetByIdAsync(command.Request.Id, cancellationToken);
        if (vote is null)
            return Result<Vote?>.Failure(null, Error.NullValue("Specified vote does not exist."));

        vote.ModifiedOn = DateTime.UtcNow;
        vote.IsPositive = command.Request.IsPositive;

        voteRepository.Update(vote);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Vote?>.Success(vote);
    }
}
