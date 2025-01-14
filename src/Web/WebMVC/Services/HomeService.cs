using Contracts.Plates;
using DTOs;
using IntegrationEvents.Plates;
using MassTransit;
using WebMVC.Models;

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

        public async Task<PaginatedViewModel<PlateDto>> GetPlatesAsync(int pageNumber, int pageSize)
        {
            var response = await _getPlatesClient.GetResponse<GetPlatesResponse>(
                new GetPlatesRequest(
                    pageSize,
                    (pageNumber - 1) * pageSize
                ));

            var paginatedResponse = PaginatedViewModel<PlateDto>.FromDto(response.Message.Response);
            return paginatedResponse;
        }

        public async Task UpsertPlateAsync(PlateDto plateDto)
        {
            var eventMessage = new UpsertPlateEvent(plateDto);
            await _publishEndpoint.Publish(eventMessage);
        }
    }
}
