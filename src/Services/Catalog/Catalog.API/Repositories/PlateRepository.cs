using Catalog.API.Mappings;
using Catalog.Domain.Interfaces;
using DTOs.Common;

namespace Catalog.API.Repositories
{
    public class PlateRepository : IPlateRepository
    {
        private readonly ApplicationDbContext _context;

        public PlateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Plate plate)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Plate>> GetAllAsync()
        {
            var plates = await _context.Plates
                .Select(p => p.FromEntity())
                .ToListAsync();
            return plates;
        }

        public async Task<PaginatedDto<Plate>> GetPaginatedAsync(int Limit, int Offset)
        public async Task<PaginatedDto<Plate>> GetAllPaginatedAsync(int Limit, int Offset)
        {
            if (Limit < 0 || Offset < 0)
            {
                throw new ArgumentException("Limit and Offset must be non-negative");
            }

            // Execute tasks sequentially to avoid DbContext concurrency issues
            var plates = await _context.Plates
                .Skip(Offset)
                .Take(Limit)
                .Select(p => p.FromEntity())
                .ToListAsync();

            var total = await _context.Plates.CountAsync();

            return new PaginatedDto<Plate>
            {
                Items = plates,
                Limit = Limit,
                Offset = Offset,
                Total = total
            };
        }

        public Task<IEnumerable<Plate>> GetAvailablePlatesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Plate?> GetByIdAsync(int id)
        {
            var plate = await _context.Plates.FindAsync(id);
            if (plate == null)
            {
                // TODO: Handle null case if needed
                return null;
            }
            return plate.FromEntity();
        }

        public Task UpdateAsync(Plate plate)
        {
            throw new NotImplementedException();
        }

        Task<Plate> IPlateRepository.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
