namespace WebMVC.Models
{
    public class HomeViewModel<T>
    {
        public PaginationStateModel PaginationState { get; set; } = new PaginationStateModel();

        public int TotalPages { get; set; }

        public IEnumerable<T> Items { get; set; } = new List<T>();
    }
}
