using Catalog.API.Mappings;
using Catalog.Domain.Interfaces;
using Contracts.Plates;
using DTOs;
using DTOs.Common;
using MassTransit;

namespace Catalog.API.Consumers
{
    public class GetPlatesConsumer : IConsumer<GetPlatesRequest>
    {
        private readonly IPlateRepository _plateRepository;

        public GetPlatesConsumer(IPlateRepository plateRepository)
        {
            _plateRepository = plateRepository;
        }

        public async Task Consume(ConsumeContext<GetPlatesRequest> consumer)
        {
            // Check for limit and offset
            var limit = consumer.Message.Limit;
            var offset = consumer.Message.Offset;

            // Fetch plates
            var page = await _plateRepository.GetPaginatedAsync(limit, offset);

            // Convert to DTOs
            var response = new PaginatedDto<PlateDto>
            {
                Items = page.Items.Select(plate => plate.ToDto()).ToList(),
                Limit = page.Limit,
                Offset = page.Offset,
                Total = page.Total
            };

            // Respond with the data
            await consumer.RespondAsync(new GetPlatesResponse
            {
                Response = response
            });
        }
    }
}