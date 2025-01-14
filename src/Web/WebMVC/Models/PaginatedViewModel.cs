using DTOs.Common;

namespace WebMVC.Models
{
    public class PaginatedViewModel<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int CurrentPage => (Offset / PageSize) + 1;
        public int Offset { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public PaginatedViewModel(IEnumerable<T> items, int totalCount, int offset, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            Offset = offset;
            PageSize = pageSize;
        }

        public static PaginatedViewModel<T> FromDto(PaginatedDto<T> paginatedDto)
        {
            return new PaginatedViewModel<T>(
                paginatedDto.Items,
                paginatedDto.Total,
                paginatedDto.Offset,
                paginatedDto.Limit
            );
        }
    }
}
