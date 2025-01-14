namespace WebMVC.Models
{
    public class PaginationStateModel
    {
        public string SortBy { get; set; } = "Id";
        public string SortDirection { get; set; } = "asc";
        public int CurrentPage { get; set; } = 1;

        public PaginationStateModel ChangeSortBy(string sortBy)
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

        public PaginationStateModel ChangePage(int pageNumber)
        {
            var newModel = Clone();
            newModel.CurrentPage = pageNumber;
            return newModel;
        }

        private PaginationStateModel Clone()
        {
            return new PaginationStateModel()
            {
                SortBy = SortBy,
                SortDirection = SortDirection,
                CurrentPage = CurrentPage
            };
        }
    }
}
