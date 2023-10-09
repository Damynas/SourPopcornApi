using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Directors.Abstractions;
using Application.Directors.Commands;
using Domain.Directors.Entities;
using Domain.Shared;

namespace Application.Directors.CommandHandlers;

public class CreateDirectorCommandHandler(IDirectorRepository directorRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateDirectorCommand, Director>
{
    public async Task<Result<Director>> Handle(CreateDirectorCommand command, CancellationToken cancellationToken)
    {
        var director = new Director(default, DateTime.UtcNow, DateTime.UtcNow)
        {
            Name = command.Request.Name,
            Country = command.Request.Country,
            BornOn = command.Request.BornOn.ToUniversalTime()
        };

        directorRepository.Add(director);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Director>.Success(director);
    }
}
