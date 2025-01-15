namespace WebMVC.Models
{
    public class PaginationStateModel
    {
        public string? SearchQuery { get; set; } = null;
        public string SortBy { get; set; } = "Id";
        public string SortDirection { get; set; } = "asc";
        public int CurrentPage { get; set; } = 1;

        public PaginationStateModel SetSearch(string search)
        {
            var newModel = Clone();
            newModel.SearchQuery = search;
            return newModel;
        }

        public PaginationStateModel SetSortBy(string sortBy)
        {
            var newModel = Clone();

            if (newModel.SortBy == sortBy)
            {
                newModel.SortDirection = newModel.SortDirection == "asc" ? "desc" : "asc";
            }
            else
            {
                newModel.SortBy = sortBy;
                newModel.SortDirection = "asc";
            }

            return newModel;
        }

        public PaginationStateModel SetPage(int pageNumber)
        {
            var newModel = Clone();
            newModel.CurrentPage = pageNumber;
            return newModel;
        }

        private PaginationStateModel Clone()
        {
            return new PaginationStateModel()
            {
                SearchQuery = SearchQuery,
                SortBy = SortBy,
                SortDirection = SortDirection,
                CurrentPage = CurrentPage
            };
        }
    }
}
