using System;
using Catalog.Domain;
using Xunit;

namespace Catalog.UnitTests.Domain
{
    public class PlateTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var registration = "Plate123";
            var purchasePrice = 1000m;
            var salePrice = 1200m;

            // Act
            var plate = new Plate(id, registration, purchasePrice, salePrice);

            // Assert
            Assert.Equal(id, plate.Id);
            Assert.Equal(registration, plate.Registration);
            Assert.Equal(purchasePrice, plate.PurchasePrice);
            Assert.Equal(salePrice, plate.SalePrice);
            Assert.Equal(1440, plate.CalculatedSalePrice);
            Assert.Equal("Plate", plate.Letters);
            Assert.Equal(123, plate.Numbers);
        }

        [Fact]
        public void Constructor_WithNullRegistration_ShouldInitializeLettersAndNumbers()
        {
            // Arrange
            var id = Guid.NewGuid();
            string registration = null;
            var purchasePrice = 1000m;
            var salePrice = 1200m;

            // Act
            var plate = new Plate(id, registration, purchasePrice, salePrice);

            // Assert
            Assert.Equal(id, plate.Id);
            Assert.Null(plate.Registration);
            Assert.Equal(purchasePrice, plate.PurchasePrice);
            Assert.Equal(salePrice, plate.SalePrice);
            Assert.Equal(string.Empty, plate.Letters);
            Assert.Equal(-1, plate.Numbers);
        }
    }
}
