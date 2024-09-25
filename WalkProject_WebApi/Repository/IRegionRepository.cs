using WalkProject_WebApi.Models.Domain;

namespace WalkProject_WebApi.Repository
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region> GetByIdAsync(Guid id);
        Task<Region> CreateAsync(Region region);
        Task<Region> UpdateAsync(Region region, Guid id);
        Task<Region> DeleteAsync(Guid id);


    }
}
