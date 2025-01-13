using DTOs.Common;

namespace Catalog.Domain.Interfaces
{
    public interface IPlateRepository
    {
        Task<Plate> GetByIdAsync(int id);
        Task<IEnumerable<Plate>> GetAllAsync();
        Task<PaginatedDto<Plate>> GetPaginatedAsync(int Limit, int Offset);
        Task<IEnumerable<Plate>> GetAvailablePlatesAsync();
        Task AddAsync(Plate plate);
        Task UpdateAsync(Plate plate);
        Task DeleteAsync(int id);
    }
}
