using Application.Abstractions.Data;
using Domain.Abstractions.Abstracts;
using Domain.Shared.Paging;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Abstractions;

public abstract class Repository<TEntity> where TEntity : Entity
{
    protected readonly IApplicationDbContext _dbContext;

    protected Repository(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<PagedList<TEntity>> GetAsync(SearchParameters searchParameters, CancellationToken cancellationToken)
    {
        var query = _dbContext.Set<TEntity>()
            .AsQueryable()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.CreatedOn);
        var pagedList = await PagedList<TEntity>.CreateAsync(query, searchParameters.PageNumber, searchParameters.PageSize, cancellationToken);
        return pagedList;
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<TEntity>()
            .SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public virtual void Add(TEntity entity)
    {
        _dbContext.Set<TEntity>().Add(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }
}