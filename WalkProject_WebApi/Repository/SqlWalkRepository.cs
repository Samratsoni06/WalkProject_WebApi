using WalkProject_WebApi.Models.Domain;
using WalkProject_WebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WalkProject_WebApi.Repository
{
    public class SqlWalkRepository : IWalkRepository
    {
        private readonly WalkProjectDbContext dbContext;
        public SqlWalkRepository(WalkProjectDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;

        }

        public async Task<Walk> DeleteAsync(Guid Id)
        {
            var existingWalk = await dbContext.Walks.Include("RegionData").Include("Difficulty").FirstOrDefaultAsync(x => x.Id == Id);
            if (existingWalk != null)
            {
                dbContext.Walks.Remove(existingWalk);
                await dbContext.SaveChangesAsync();
                return existingWalk;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Walk>> GetWalkAsync(string? filteron = null, string? filterquery = null, string? SortBy = null, bool isAcending = true, int PageNumber = 1, int PageSize = 1000)
        {
            //return await dbContext.Walks.ToListAsync();
            //return await dbContext.Walks.Include("RegionData").Include("Difficulty").ToListAsync();

            var walks = dbContext.Walks.Include("RegionData").Include("Difficulty").AsQueryable();

            if(string.IsNullOrWhiteSpace(filteron)==false && string.IsNullOrWhiteSpace(filterquery)==false)
            {
                if(filteron.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterquery));
                }              
            }

            //Sort
            if (string.IsNullOrWhiteSpace(SortBy) == false)
            {
                if (SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAcending?walks.OrderBy(x => x.Name) :walks.OrderByDescending(x => x.Name);
                }
                else if (SortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAcending?walks.OrderBy(x=>x.LengthInKm) : walks.OrderByDescending(x=>x.LengthInKm);
                }
            }
            //Pagging
            var SkipPage = (PageNumber - 1) * PageSize;
            return await walks.Skip(SkipPage).Take(PageSize).ToListAsync();
                //return await walks.ToListAsync();

        }

        public async Task<Walk> GetWalkByIdAsync(Guid Id)
        {
            var walkDom = await dbContext.Walks.Include("RegionData").Include("Difficulty").FirstOrDefaultAsync(x => x.Id == Id);
            if (walkDom == null)
            {
                return null;
            }
            return (walkDom);
        }

        public async Task<Walk> UpdateAsync(Walk walk, Guid Id)
        {
            var existingWalk = await dbContext.Walks.Include("RegionData").Include("Difficulty").FirstOrDefaultAsync(x=>x.Id == Id);
            if(existingWalk == null)
            {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.DifficultyId = walk.DifficultyId;
            await dbContext.SaveChangesAsync();
            return existingWalk;

        } 
    }
}
