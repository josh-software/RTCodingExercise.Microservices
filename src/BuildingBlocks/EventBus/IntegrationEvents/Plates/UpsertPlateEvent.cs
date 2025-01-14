using DTOs;

namespace IntegrationEvents.Plates
{
    public class UpsertPlateEvent : IntegrationEvent
    {
        public PlateDto Plate { get; set; }

        public UpsertPlateEvent(PlateDto plate)
        {
            Plate = plate;
        }
    }
}