namespace WebMVC.Models
{
    public class PaginationStateModel
    {
        public string SortBy { get; set; } = "Id";
        public string SortDirection { get; set; } = "asc";
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
    }
}
