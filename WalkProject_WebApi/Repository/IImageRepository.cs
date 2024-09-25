using WalkProject_WebApi.Models.Domain;

namespace WalkProject_WebApi.Repository
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
