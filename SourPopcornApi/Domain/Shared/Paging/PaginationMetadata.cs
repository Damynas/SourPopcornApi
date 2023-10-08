namespace Domain.Shared.Paging;

public record PaginationMetadata(int TotalCount, int PageSize, int CurrentPage);
