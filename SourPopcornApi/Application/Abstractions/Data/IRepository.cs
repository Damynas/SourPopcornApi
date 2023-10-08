using Domain.Abstractions.Interfaces;
using Domain.Shared.Paging;

namespace Application.Abstractions.Data;

public interface IRepository<TEntity> where TEntity : IEntity
{
    Task<PagedList<TEntity>> GetAsync(SearchParameters searchParameters, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    void Add(TEntity entity);

    void Update(TEntity entity);

    void Remove(TEntity entity);
}
