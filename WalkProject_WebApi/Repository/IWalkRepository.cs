using WalkProject_WebApi.Models.Domain;

namespace WalkProject_WebApi.Repository
{
    public interface IWalkRepository
    {
        //Task<List<Walk>> GetWalkAsync();

        //Add Filter
        //Add Sort
        //Add Paging
        Task<List<Walk>> GetWalkAsync(string? filteron = null , string? filterquery = null, string? SortBy = null, bool isAcending = true,int PageNumber = 1, int PageSize = 1000);


        Task<Walk> GetWalkByIdAsync(Guid Id);
        Task<Walk> CreateAsync(Walk walk);
        Task<Walk> UpdateAsync(Walk walk,Guid Id);
        Task<Walk> DeleteAsync(Guid Id);
    }
}
