namespace WalkProject_WebApi.Models.Domain
{
    public class Region
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public string? RegionalImageUrl { get; set; }
    }
}
