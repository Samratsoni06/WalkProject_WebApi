using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WalkProject_WebApi.Data;
using WalkProject_WebApi.Models.Domain;

namespace WalkProject_WebApi.Repository
{
    public class SqlRegionRepository : IRegionRepository
    {
        private readonly WalkProjectDbContext dbContext;

        public SqlRegionRepository(WalkProjectDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;

        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var ExistingRegion = await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);
            if (ExistingRegion == null)
            {
                return null;
            }
            dbContext.Regions.Remove(ExistingRegion);
            await dbContext.SaveChangesAsync();
            return ExistingRegion;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetByIdAsync(Guid id)
        {
            var result = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<Region> UpdateAsync(Region region, Guid id)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            {
                return null;
            }
            existingRegion.Name = region.Name;
            existingRegion.Code = region.Code;
            existingRegion.RegionalImageUrl = region.RegionalImageUrl;
            await dbContext.SaveChangesAsync();
            return existingRegion;

        }
    }
}
