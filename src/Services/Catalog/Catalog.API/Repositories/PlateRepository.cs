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

        public async Task AddAsync(Plate plate)
        {
            if (plate == null)
            {
                throw new ArgumentNullException(nameof(plate));
            }

            await _context.Plates.AddAsync(plate.ToEntity());
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var plate = await _context.Plates.FindAsync(id);
            if (plate == null)
            {
                throw new KeyNotFoundException($"Plate with id {id} not found.");
            }

            _context.Plates.Remove(plate);
            await _context.SaveChangesAsync();
        }

        public async Task<Plate?> GetAsync(Guid id)
        {
            var plate = await _context.Plates.FindAsync(id);
            if (plate == null)
            {
                // TODO: Handle null case if needed
                return null;
            }
            return plate.FromEntity();
        }

        public async Task UpdateAsync(Plate plate)
        {
            if (plate == null)
            {
                throw new ArgumentNullException(nameof(plate));
            }

            var existingPlate = await _context.Plates.FindAsync(plate.Id);
            if (existingPlate == null)
            {
                throw new KeyNotFoundException($"Plate with id {plate.Id} not found.");
            }

            _context.Entry(existingPlate).CurrentValues.SetValues(plate.ToEntity());
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedDto<Plate>> GetAllPaginatedAsync(int limit, int offset)
        {
            if (limit < 0 || offset < 0)
            {
                throw new ArgumentException("Limit and Offset must be non-negative");
            }

            var plates = await _context.Plates
                .OrderBy(p => p.Id)
                .Skip(offset)
                .Take(limit)
                .Select(p => p.FromEntity())
                .ToListAsync();

            var total = await _context.Plates.CountAsync();

            return new PaginatedDto<Plate>
            {
                Items = plates,
                Limit = limit,
                Offset = offset,
                Total = total
            };
        }
    }
}
