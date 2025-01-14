using DTOs.Common;

namespace Catalog.Domain.Interfaces
{
    public interface IPlateRepository
    {
        Task AddAsync(Plate plate);
        Task UpdateAsync(Plate plate);
        Task DeleteAsync(Guid id);
        Task<Plate?> GetAsync(Guid id);
        Task<PaginatedDto<Plate>> GetAllPaginatedAsync(int Limit, int Offset);
    }
}
