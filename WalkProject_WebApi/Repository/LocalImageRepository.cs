using WalkProject_WebApi.Data;
using WalkProject_WebApi.Models.Domain;

namespace WalkProject_WebApi.Repository
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly WalkProjectDbContext dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, WalkProjectDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            var LocalFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", image.FileName + image.FileExtension);
            var Stream = new FileStream(LocalFilePath,FileMode.Create);
            await image.File.CopyToAsync(Stream);
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileExtension}";
            image.FilePath = urlFilePath;
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();
            return image;

        }
    }
}
