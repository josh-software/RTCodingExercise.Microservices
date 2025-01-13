using DTOs;
using DTOs.Common;

namespace Contracts.Plates
{
    public class GetPlatesRequest
    {
        public int Limit { get; set; }
        public int Offset { get; set; }

        public GetPlatesRequest(int limit, int offset)
        {
            Limit = limit;
            Offset = offset;
        }
    }

    public class GetPlatesResponse
    {
        public PaginatedDto<PlateDto> Response { get; set; } = new PaginatedDto<PlateDto>();
    }
}
