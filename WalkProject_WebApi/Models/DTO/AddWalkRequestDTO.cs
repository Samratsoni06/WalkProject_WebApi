using System.ComponentModel.DataAnnotations;

namespace WalkProject_WebApi.Models.DTO
{
    public class AddWalkRequestDTO
    {
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public string Description { get; set; } = "";
        [Required]
        [Range(05,60,ErrorMessage ="Range between 05 to 60 Km")]
        public string LengthInKm { get; set; } = "";
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid RegionId { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
    }
}
