using Domain.Abstractions.Interfaces;
using Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Domain.Shared.Paging;

public class PagedList<TEntity> where TEntity : IEntity
{
    public List<TEntity> Items { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage * PageSize < TotalCount;

    private PagedList(List<TEntity> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        CurrentPage = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public static async Task<PagedList<TEntity>> CreateAsync(IQueryable<TEntity> query, int page, int pageSize, CancellationToken cancellationToken)
    {
        if (page < Default.PageNumber)
            page = Default.PageNumber;

        if (pageSize > Default.MaximumPageSize)
            pageSize = Default.MaximumPageSize;

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new(items, page, pageSize, totalCount);
    }
}
