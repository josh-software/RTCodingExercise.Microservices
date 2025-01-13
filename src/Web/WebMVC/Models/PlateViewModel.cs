namespace WebMVC.Models
{
    public class PlateViewModel
    {
        public string Registration { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }

        public decimal MarkedUpSalePrice => PurchasePrice * 1.20m;
    }

}
