using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Directors.Abstractions;
using Application.Directors.Commands;
using Domain.Directors.Entities;
using Domain.Shared;

namespace Application.Directors.CommandHandlers;

public class UpdateDirectorCommandHandler(IDirectorRepository directorRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateDirectorCommand, Director?>
{
    public async Task<Result<Director?>> Handle(UpdateDirectorCommand command, CancellationToken cancellationToken)
    {
        var director = await directorRepository.GetByIdAsync(command.Request.DirectorId, cancellationToken);
        if (director is null)
            return Result<Director?>.Failure(null, Error.NullValue("Specified director does not exist."));

        director.ModifiedOn = DateTime.UtcNow;
        director.Name = command.Request.Name;
        director.Country = command.Request.Country;
        director.BornOn = command.Request.BornOn.ToUniversalTime();

        directorRepository.Update(director);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Director?>.Success(director);
    }
}
