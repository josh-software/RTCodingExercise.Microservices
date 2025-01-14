namespace WebMVC.Models
{
    public class HomeViewModel<T> : PaginationStateModel
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
    }
}
