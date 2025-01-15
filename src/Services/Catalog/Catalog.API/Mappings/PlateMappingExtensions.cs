namespace Catalog.API.Mappings
{
    using Catalog.Domain;
    using DTOs;

    public static class PlateMappingExtensions
    {
        public static PlateDto ToDto(this Plate plate)
        {
            return new PlateDto
            {
                Id = plate.Id,
                Registration = plate.Registration ?? string.Empty,
                PurchasePrice = plate.PurchasePrice,
                SalePrice = plate.SalePrice,
                CalculatedSalePrice = plate.CalculatedSalePrice
            };
        }

        public static Plate FromDto(this PlateDto plateDto)
        {
            return new Plate(
                plateDto.Id ?? Guid.NewGuid(),
                plateDto.Registration,
                plateDto.PurchasePrice,
                plateDto.SalePrice);
        }

        public static IEnumerable<PlateDto> ToDtos(this IEnumerable<Plate> plates)
        {
            return plates.Select(plate => plate.ToDto());
        }

        public static PlateEntity ToEntity(this Plate plate)
        {
            return new PlateEntity
            {
                Id = plate.Id,
                Registration = plate.Registration,
                PurchasePrice = plate.PurchasePrice,
                SalePrice = plate.SalePrice,
                Letters = plate.Letters,
                Numbers = plate.Numbers,
                IsReserved = plate.IsReserved
            };
        }

        public static Plate FromEntity(this PlateEntity plateEntity)
        {
            return new Plate(
                plateEntity.Id,
                plateEntity.Registration,
                plateEntity.PurchasePrice,
                plateEntity.SalePrice,
                plateEntity.Letters,
                plateEntity.Numbers
            );
        }
    }
}
