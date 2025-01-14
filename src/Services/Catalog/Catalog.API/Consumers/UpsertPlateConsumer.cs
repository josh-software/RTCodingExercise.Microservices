using Catalog.API.Mappings;
using Catalog.Domain.Interfaces;
using IntegrationEvents.Plates;
using MassTransit;

namespace Catalog.API.Consumers
{
    public class UpsertPlateConsumer
    {
        private readonly IPlateRepository _plateRepository;

        public UpsertPlateConsumer(IPlateRepository plateRepository)
        {
            _plateRepository = plateRepository;
        }

        public async Task Consume(ConsumeContext<UpsertPlateEvent> context)
        {
            var plateDto = context.Message.Plate;

            // Check if plate exists
            var plate = await _plateRepository.GetAsync(plateDto.Id);
            if (plate == null)
            {
                // Add new plate
                await _plateRepository.AddAsync(plateDto.FromDto());
            }
            else
            {
                // Update existing plate
                await _plateRepository.UpdateAsync(plateDto.FromDto());
            }
        }
    }
}
