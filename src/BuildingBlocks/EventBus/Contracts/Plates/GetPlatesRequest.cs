using DTOs;
using DTOs.Common;

namespace Contracts.Plates
{
    public class GetPlatesRequest
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string? SearchQuery { get; set; }
        public SortBy Sort { get; set; } = SortBy.Id;
        public SortDirection Direction { get; set; } = SortDirection.Asc;

        public enum SortBy
        {
            Id,
            Registration,
            PurchasePrice,
            SalePrice
        }

        public enum SortDirection
        {
            Asc,
            Desc
        }

        public GetPlatesRequest() { }

        public GetPlatesRequest(int limit, int offset, string? searchQuery, string sortBy, string sortDirection)
        {
            if (!Enum.TryParse(sortBy, ignoreCase: true, out SortBy sortByEnum))
                throw new ArgumentException($"Invalid value for {nameof(sortBy)}", nameof(sortBy));

            if (!Enum.TryParse(sortDirection, ignoreCase: true, out SortDirection sortDirectionEnum))
                throw new ArgumentException($"Invalid value for {nameof(sortDirection)}", nameof(sortDirection));

            Limit = limit;
            Offset = offset;
            SearchQuery = searchQuery;
            Sort = sortByEnum;
            Direction = sortDirectionEnum;
        }
    }

    public class GetPlatesResponse
    {
        public PaginatedDto<PlateDto> Response { get; set; } = new PaginatedDto<PlateDto>();
    }
}
