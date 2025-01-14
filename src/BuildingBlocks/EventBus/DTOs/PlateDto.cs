namespace DTOs
{
    public class PlateDto
    {
        public Guid? Id { get; set; } = null;
        public string Registration { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? CalculatedSalePrice { get; set; } = null;
    }
}
