using Domain.Directors.DataTransferObjects.Requests;
using Domain.Directors.Entities;
using Domain.Shared;
using Domain.Shared.Paging;

namespace Application.Directors.Abstractions;

public interface IDirectorService
{
    Task<Result<PagedList<Director>>> GetDirectorsAsync(GetDirectorsRequest request, CancellationToken cancellationToken = default);
    Task<Result<Director?>> GetDirectorByIdAsync(GetDirectorByIdRequest request, CancellationToken cancellationToken = default);
    Task<Result<Director>> CreateDirectorAsync(CreateDirectorRequest request, CancellationToken cancellationToken = default);
    Task<Result<Director?>> UpdateDirectorAsync(UpdateDirectorRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteDirectorAsync(DeleteDirectorRequest request, CancellationToken cancellationToken = default);
}
