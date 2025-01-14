using Contracts.Plates;
using DTOs;
using DTOs.Common;
using IntegrationEvents.Plates;
using MassTransit;

namespace WebMVC.Services
{
    public class HomeService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<GetPlatesRequest> _getPlatesClient;

        public HomeService(
            IPublishEndpoint publishEndpoint,
            IRequestClient<GetPlatesRequest> getPlatesClient)
        {
            _publishEndpoint = publishEndpoint;
            _getPlatesClient = getPlatesClient;
        }

        public async Task<PaginatedDto<PlateDto>> GetPlatesAsync(
            int pageNumber, int pageSize, string sortBy, string sortDirection
        )
        {
            var response = await _getPlatesClient.GetResponse<GetPlatesResponse>(
                new GetPlatesRequest(
                    pageSize,
                    (pageNumber - 1) * pageSize,
                    sortBy,
                    sortDirection
                ));

            return response.Message.Response;
        }

        public async Task UpsertPlateAsync(PlateDto plateDto)
        {
            var eventMessage = new UpsertPlateEvent(plateDto);
            await _publishEndpoint.Publish(eventMessage);
        }
    }
}
