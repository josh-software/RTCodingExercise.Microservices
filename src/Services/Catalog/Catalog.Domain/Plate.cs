namespace Catalog.Domain
{
    public class Plate
    {
        public Guid Id { get; set; }

        public string? Registration { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice { get; set; }

        public string? Letters { get; set; }

        public int Numbers { get; set; }

        public decimal CalculatedSalePrice => CalculateSalePrice(PurchasePrice);

        private static decimal CalculateSalePrice(decimal salePrice)
        {
            return RoundUp(salePrice * 1.20m, 2);
        }

        private static decimal RoundUp(decimal value, int decimalPlaces)
        {
            var multiplier = (decimal)Math.Pow(10, decimalPlaces);
            return Math.Ceiling(value * multiplier) / multiplier;
        }
    }
}