namespace DTOs.Common
{
    public class PaginatedDto<T>
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; } = new List<T>();
    }
}
