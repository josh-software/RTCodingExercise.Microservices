using System.Linq.Dynamic.Core;
using Catalog.API.Helpers;
using Catalog.API.Mappings;
using Catalog.Domain.Interfaces;
using DTOs.Common;
using LinqKit;

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

        public async Task<PaginatedDto<Plate>> GetAllPaginatedAsync(int limit, int offset, string? searchQuery, string sortBy, string sortDirection)
        {
            if (limit < 0 || offset < 0)
            {
                throw new ArgumentException("Limit and Offset must be non-negative");
            }

            List<Plate> plates;
            int total;

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                plates = await _context.Plates
                    .OrderBy($"{sortBy} {sortDirection}")
                    .Skip(offset)
                    .Take(limit)
                    .Select(p => p.FromEntity())
                    .ToListAsync();

                total = await _context.Plates.CountAsync();
            }
            else
            {
                var context = _context.Plates
                    .Where(p => p.Registration != null);

                context = ApplySearchFilter(context, searchQuery);

                plates = await context
                    .OrderBy($"{sortBy} {sortDirection}") //TODO: Ideally we should be able to sort by relevance, like a simple similarity score
                    .Skip(offset)
                    .Take(limit)
                    .Select(p => p.FromEntity())
                    .ToListAsync();

                total = await context.CountAsync();
            }

            return new PaginatedDto<Plate>
            {
                Items = plates,
                Limit = limit,
                Offset = offset,
                Total = total
            };
        }

        // TODO: Having all this logic in the repository is not ideal, and should be moved to a service, but for now it's fine
        // TODO: This query is very expensive, and should ideally be denormalised to optimise search
        // e.g. Removing whitespace and nulls in advance and creating a new lookup table for substrings
        // i.e. "AB 12" -> "AB12", "B12", "12", "2" and the ID, so we can index and use a simple LIKE 'search%'/StartWith query, rather than LIKE '%search%'/Contains
        private IQueryable<PlateEntity> ApplySearchFilter(IQueryable<PlateEntity> query, string searchQuery)
        {
            searchQuery = searchQuery.Replace(" ", "");
            var searchTerms = LettersToNumbers.GenerateCombinations(searchQuery);

            var predicate = PredicateBuilder.New<PlateEntity>();
            // Check if the search query is in the letters or numbers
            predicate = predicate
                .Or(e => e.Letters!.Contains(searchQuery))
                .Or(e => e.Numbers!.ToString().Contains(searchQuery));

            // Substituting the search query with all possible combinations of lookalike letters
            // TODO: Something similar but the other way around for numbers
            foreach (var term in searchTerms)
            {
                predicate = predicate.Or(e => e.Registration!.Replace(" ", "").Contains(term));
            }

            return query.AsExpandable().Where(predicate);
        }
    }
}
