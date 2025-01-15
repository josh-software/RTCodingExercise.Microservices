using Catalog.Domain.Interfaces;
using IntegrationEvents.Plates;
using MassTransit;

namespace Catalog.API.Consumers
{
    public class ReservePlateConsumer : IConsumer<ReservePlateEvent>
    {
        private readonly IPlateRepository _plateRepository;
        private readonly ILogger<ReservePlateConsumer> _logger;

        public ReservePlateConsumer(IPlateRepository plateRepository, ILogger<ReservePlateConsumer> logger)
        {
            _plateRepository = plateRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ReservePlateEvent> context)
        {
            var plateId = context.Message.PlateId;

            var plate = await _plateRepository.GetAsync(plateId);

            if (plate is null)
            {
                _logger.LogWarning("Plate with id {PlateId} not found.", plateId);
                return;
            }

            if (plate.IsReserved)
            {
                _logger.LogWarning("Plate with id {PlateId} is already reserved.", plateId);
                return;
            }

            plate.IsReserved = true;
            await _plateRepository.UpdateAsync(plate);

            _logger.LogInformation("Plate with id {PlateId} has been reserved.", plateId);
        }
    }
}
