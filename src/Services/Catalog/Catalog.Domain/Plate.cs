﻿namespace Catalog.Domain
{
    public class Plate
    {
        public Guid Id { get; set; }

        public string? Registration { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice { get; set; }

        public string? Letters { get; set; }

        public int Numbers { get; set; }

        public bool IsReserved { get; set; }

        public decimal CalculatedSalePrice => CalculateSalePrice(SalePrice);

        public Plate(
            Guid id,
            string? registration,
            decimal purchasePrice,
            decimal salePrice,
            string? letters = null,
            int? numbers = null,
            bool isReserved = false)
        {
            Id = id;
            Registration = registration;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            Letters = letters ?? getLetters(registration);
            Numbers = numbers ?? getNumbers(registration);
            IsReserved = isReserved;
        }

        private static string getLetters(string? registration)
        {
            if (string.IsNullOrEmpty(registration))
                return string.Empty;

            return new string(registration.Where(char.IsLetter).ToArray());
        }

        // TODO - JS: Need to figure out how to handle plates without numbers
        // TODO - JS: Can a plate even have no numbers?
        private static int getNumbers(string? registration)
        {
            if (string.IsNullOrEmpty(registration))
                return -1;

            var numbersPart = new string(registration.Where(char.IsDigit).ToArray());
            return int.TryParse(numbersPart, out var result) ? result : -1;
        }

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