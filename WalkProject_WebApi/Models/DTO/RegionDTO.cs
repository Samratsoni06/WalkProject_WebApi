﻿namespace WalkProject_WebApi.Models.DTO
{
    public class RegionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public string? RegionalImageUrl { get; set; } 
    }
}
