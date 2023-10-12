using Application.Abstractions.Messaging;
using Application.Directors.Abstractions;
using Application.Directors.Queries;
using Domain.Directors.Entities;
using Domain.Shared;

namespace Application.Directors.QueryHandlers;

public class GetDirectorByIdQueryHandler(IDirectorRepository directorRepository) : IQueryHandler<GetDirectorByIdQuery, Director?>
{
    public async Task<Result<Director?>> Handle(GetDirectorByIdQuery query, CancellationToken cancellationToken)
    {
        var director = await directorRepository.GetByIdAsync(query.Request.DirectorId, cancellationToken);
        if (director is null)
            return Result<Director?>.Failure(null, Error.NullValue("Specified director does not exist."));

        return Result<Director?>.Success(director);
    }
}
