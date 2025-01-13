using Catalog.Domain;
using Catalog.Domain.Interfaces;

namespace Catalog.Infrastructure.Repositories
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

        public Task<IEnumerable<Plate>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Plate>> GetAvailablePlatesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Plate> GetByIdAsync(int id)
        {
            return await _context.Plates.FindAsync(id);
        }

        public Task UpdateAsync(Plate plate)
        {
            throw new NotImplementedException();
        }

        Task<Plate> IPlateRepository.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        // Other methods here...
    }
}
