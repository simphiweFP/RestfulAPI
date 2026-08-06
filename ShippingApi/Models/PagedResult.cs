namespace ShippingApi.Models
{
    public class PagedResult<T>
    {
        public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize, int totalPages)
        {
            Items = items.ToList();
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
        }

        public IReadOnlyCollection<T> Items { get; }
        public int TotalCount { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}