using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Votes.Abstractions;
using Application.Votes.Commands;
using Domain.Shared;

namespace Application.Votes.CommandHandlers;

public class DeleteVoteCommandHandler(IVoteRepository voteRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteVoteCommand>
{
    public async Task<Result> Handle(DeleteVoteCommand command, CancellationToken cancellationToken)
    {
        var vote = await voteRepository.GetByIdAsync(command.Request.Id, cancellationToken);
        if (vote is null)
            return Result.Failure(Error.NullValue("Specified vote does not exist."));

        vote.ModifiedOn = DateTime.UtcNow;
        vote.IsDeleted = true;

        voteRepository.Update(vote);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
