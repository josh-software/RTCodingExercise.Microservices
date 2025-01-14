using DTOs.Common;

namespace WebMVC.Models
{
    public class HomeViewModel<T> : PaginationStateModel
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();

        public void AddItemsFromPaginatedDto(PaginatedDto<T> paginatedDto)
        {
            Items = paginatedDto.Items;
        }
    }
}
