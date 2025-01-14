using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.API.Mappings;
using Catalog.Domain;
using DTOs;
using Xunit;

namespace Catalog.API.Tests.Mappings
{
    public class PlateMappingExtensionsTests
    {
        [Fact]
        public void ToDto_ShouldMapPlateToPlateDto()
        {
            // Arrange
            var plate = new Plate(
                Guid.NewGuid(),
                "Plate1",
                1000m,
                1200m
            );

            // Act
            var plateDto = plate.ToDto();

            // Assert
            Assert.Equal(plate.Id, plateDto.Id);
            Assert.Equal(plate.Registration, plateDto.Registration);
            Assert.Equal(plate.PurchasePrice, plateDto.PurchasePrice);
            Assert.Equal(plate.SalePrice, plateDto.SalePrice);
            Assert.NotEqual(plateDto.SalePrice, plateDto.CalculatedSalePrice);
            Assert.Equal(plate.CalculatedSalePrice, plateDto.CalculatedSalePrice);
        }

        [Fact]
        public void FromDto_ShouldMapPlateDtoToPlate()
        {
            // Arrange
            var plateDto = new PlateDto
            {
                Id = Guid.NewGuid(),
                Registration = "Plate1",
                PurchasePrice = 500m,
                SalePrice = 600m,
                CalculatedSalePrice = 650m
            };

            // Act
            var plate = plateDto.FromDto();

            // Assert
            Assert.Equal(plateDto.Id, plate.Id);
            Assert.Equal(plateDto.Registration, plate.Registration);
            Assert.Equal(plateDto.PurchasePrice, plate.PurchasePrice);
            Assert.Equal(plateDto.SalePrice, plate.SalePrice);
        }

        [Fact]
        public void ToDtos_ShouldMapListOfPlatesToListOfPlateDtos()
        {
            // Arrange
            var plates = new List<Plate>
            {
                new Plate(Guid.NewGuid(), "Plate1", 1000m, 1200m),
                new Plate(Guid.NewGuid(), "Plate2", 1500m, 1800m)
            };

            // Act
            var plateDtos = plates.ToDtos();

            // Assert
            Assert.Equal(plates.Count, plateDtos.Count());
            Assert.All(plateDtos, plateDto =>
            {
                Assert.NotNull(plateDto.Registration);
                Assert.True(plateDto.PurchasePrice > 0);
                Assert.True(plateDto.SalePrice > 0);
            });
        }

        [Fact]
        public void ToEntity_ShouldMapPlateToPlateEntity()
        {
            // Arrange
            var plate = new Plate(
                Guid.NewGuid(),
                "Plate123",
                2000m,
                2200m
            );

            // Act
            var plateEntity = plate.ToEntity();

            // Assert
            Assert.Equal(plate.Id, plateEntity.Id);
            Assert.Equal(plate.Registration, plateEntity.Registration);
            Assert.Equal(plate.PurchasePrice, plateEntity.PurchasePrice);
            Assert.Equal(plate.SalePrice, plateEntity.SalePrice);
            Assert.Equal("Plate", plateEntity.Letters);
            Assert.Equal(123, plateEntity.Numbers);
        }

        [Fact]
        public void FromEntity_ShouldMapPlateEntityToPlate()
        {
            // Arrange
            var plateEntity = new PlateEntity
            {
                Id = Guid.NewGuid(),
                Registration = "Plate123",
                PurchasePrice = 2500m,
                SalePrice = 2700m,
                Letters = "Plate",
                Numbers = 123
            };

            // Act
            var plate = plateEntity.FromEntity();

            // Assert
            Assert.Equal(plateEntity.Id, plate.Id);
            Assert.Equal(plateEntity.Registration, plate.Registration);
            Assert.Equal(plateEntity.PurchasePrice, plate.PurchasePrice);
            Assert.Equal(plateEntity.SalePrice, plate.SalePrice);
            Assert.Equal(plateEntity.Letters, plate.Letters);
            Assert.Equal(plateEntity.Numbers, plate.Numbers);
        }
    }
}
