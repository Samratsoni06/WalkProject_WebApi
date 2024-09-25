using AutoMapper;
using WalkProject_WebApi.Models.Domain;
using WalkProject_WebApi.Models.DTO;

namespace WalkProject_WebApi.Mapping
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<AddRegionRequestDto,Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();

            // For Walk Controller
            CreateMap<AddWalkRequestDTO, Walk>().ReverseMap();
            CreateMap<Walk,WalkDTO>().ReverseMap();
            CreateMap<Difficulty,DifficultyDTO>().ReverseMap();
            CreateMap<Region, RegionDT>().ReverseMap();
            CreateMap<UpdateWalkDTO,Walk>().ReverseMap();
        }

    }
}
