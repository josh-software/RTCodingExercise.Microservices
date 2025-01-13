using Contracts.Plates;
using DTOs;
using MassTransit;
using WebMVC.Models;

namespace WebMVC.Services
{

    public class HomeService
    {
        private readonly IRequestClient<GetPlatesRequest> _client;

        public HomeService(IRequestClient<GetPlatesRequest> client)
        {
            _client = client;
        }

        public async Task<PaginatedResponse<PlateDto>> GetPlatesAsync(int pageNumber, int pageSize)
        {
            var response = await _client.GetResponse<GetPlatesResponse>(
                new GetPlatesRequest(
                    pageSize,
                    (pageNumber - 1) * pageSize
                ));
            var paginatedResponse = PaginatedResponse<PlateDto>.FromDto(response.Message.Response);
            return paginatedResponse;
        }
    }
}
