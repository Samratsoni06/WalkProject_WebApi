using System.ComponentModel.DataAnnotations;

namespace WalkProject_WebApi.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required(ErrorMessage ="Please Fill The Name")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage ="Pls Enter Code")]
        [MinLength(3,ErrorMessage ="Code has been min 3 char")]
        public string Code { get; set; } = "";
        public string? RegionalImageUrl { get; set; }
    }
}
