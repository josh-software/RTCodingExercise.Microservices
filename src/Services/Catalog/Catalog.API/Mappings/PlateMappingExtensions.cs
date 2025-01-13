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
                Numbers = plate.Numbers
            };
        }

        public static Plate FromEntity(this PlateEntity plateEntity)
        {
            return new Plate
            {
                Id = plateEntity.Id,
                Registration = plateEntity.Registration,
                PurchasePrice = plateEntity.PurchasePrice,
                SalePrice = plateEntity.SalePrice,
                Letters = plateEntity.Letters,
                Numbers = plateEntity.Numbers
            };
        }
    }
}
