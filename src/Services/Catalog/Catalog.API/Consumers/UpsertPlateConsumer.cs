﻿using Catalog.API.Mappings;
using Catalog.Domain.Interfaces;
using IntegrationEvents.Plates;
using MassTransit;

namespace Catalog.API.Consumers
{
    public class UpsertPlateConsumer : IConsumer<UpsertPlateEvent>
    {
        private readonly IPlateRepository _plateRepository;

        public UpsertPlateConsumer(IPlateRepository plateRepository)
        {
            _plateRepository = plateRepository;
        }

        public async Task Consume(ConsumeContext<UpsertPlateEvent> context)
        {
            var plateDto = context.Message.Plate;

            if (plateDto.Id is not null && await _plateRepository.GetAsync((Guid)plateDto.Id) is not null)
            {
                // Update existing plate
                await _plateRepository.UpdateAsync(plateDto.FromDto());
            }
            else
            {
                // Add new plate
                // TODO: Need to add validation logic to make sure we're adding a legitmate plate, i.e. Not a duplicate registration valid price etc.
                await _plateRepository.AddAsync(plateDto.FromDto());
            }
        }
    }
}
