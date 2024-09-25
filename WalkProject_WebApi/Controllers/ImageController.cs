using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WalkProject_WebApi.Models.Domain;
using WalkProject_WebApi.Models.DTO;
using WalkProject_WebApi.Repository;

namespace WalkProject_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository imageRepository;
        public ImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO requestDTO)
        {
            ValidateFileUpload(requestDTO);
            //Convert DTO to Doman Model

            var imageDomainModel = new Image { 
                File = requestDTO.File,
                FileName = requestDTO.FileName,
                FileDescription = requestDTO.FileDescription,
                FileExtension = Path.GetExtension(requestDTO.File.FileName),
                FileSizeBytes = requestDTO.File.Length,

            };

            await imageRepository.Upload(imageDomainModel);

            return Ok(imageDomainModel);
        }

        private void ValidateFileUpload(ImageUploadRequestDTO requestDTO)
        {
            var allowedExtension = new string[] { ".jpg", ".png", ".jpeg" };
            if(allowedExtension.Contains(Path.GetExtension(requestDTO.File.FileName)))
            {
                ModelState.AddModelError("File", "File formte not Matche");
            }
            if(requestDTO.File.Length> 10485760)
            {
                ModelState.AddModelError("File", "File Shud be max 10 Mb");
            }
        }
    }
}
