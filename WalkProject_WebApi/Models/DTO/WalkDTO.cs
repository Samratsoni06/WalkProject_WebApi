namespace WalkProject_WebApi.Models.DTO
{
    public class WalkDTO
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string LengthInKm { get; set; } = "";
        public string? WalkImageUrl { get; set; }

        //public Guid RegionId { get; set; }
        //public Guid DifficultyId { get; set; }
        public RegionDT RegionData { get; set; }
        public DifficultyDTO Difficulty { get; set; }
        
    }
}
