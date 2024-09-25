using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalkProject_WebApi.CustomActionFilter;
using WalkProject_WebApi.Data;
using WalkProject_WebApi.Models.Domain;
using WalkProject_WebApi.Models.DTO;
using WalkProject_WebApi.Repository;

namespace WalkProject_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly WalkProjectDbContext _context;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<WalkProjectDbContext> logger;

        public RegionsController(WalkProjectDbContext context, IRegionRepository regionRepository, IMapper mapper, ILogger<WalkProjectDbContext> logger)
        {
            _context = context;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }




        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            try
            {
                // throw new Exception("This is errer");
                var reginsDomain = await regionRepository.GetAllAsync();
                return Ok(mapper.Map<List<RegionDTO>>(reginsDomain));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }

            //var regionDto = new List<RegionDTO>();
            //foreach (var reg in reginsDomain)
            //{
            //    regionDto.Add(new RegionDTO() { 
            //        Id = reg.Id,
            //        Name = reg.Name,
            //        Code = reg.Code,
            //        RegionalImageUrl = reg.RegionalImageUrl

            //    });
            //}
            //return Ok(regionDto);

        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var region = await regionRepository.GetByIdAsync(id);
                if (region == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<RegionDTO>(region));
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }

            //var regionDto = new RegionDTO()
            //{
            //    Id = region.Id,
            //    Name = region.Name,
            //    Code = region.Code,
            //    RegionalImageUrl = region.RegionalImageUrl               
            //};
            //return Ok(regionDto);
        }

        [HttpPost]
        [ValidationModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto Dto)
        {
            //Convert DTo to Model
            //if (ModelState.IsValid)
            //{
                var regionDmainModel = mapper.Map<Region>(Dto);

                //var regionDmainModel = new Region
                //{
                //    Code = Dto.Code,
                //    Name = Dto.Name,
                //    RegionalImageUrl = Dto.RegionalImageUrl
                //};
                // Domaim Model Call to create Design

                regionDmainModel = await regionRepository.CreateAsync(regionDmainModel);

                var regionDto = mapper.Map<Region>(Dto);

                // Back to DTO
                //var regionDto = new RegionDTO()
                //{
                //    Name = Dto.Name,
                //    Code = Dto.Code,
                //    RegionalImageUrl = Dto.RegionalImageUrl
                //};

                return CreatedAtAction(nameof(GetById), new { id = regionDmainModel.Id }, regionDmainModel);
            //}
            //else
            //{   
            //    return BadRequest(ModelState);
            //}
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //if (ModelState.IsValid)
            //{
                var reginsDomainModel = mapper.Map<Region>(updateRegionRequestDto);
                //var reginsDomainModel = new Region
                //{
                //    Name = updateRegionRequestDto.Name,
                //    Code = updateRegionRequestDto.Code,
                //    RegionalImageUrl = updateRegionRequestDto.RegionalImageUrl
                //};
                reginsDomainModel = await regionRepository.UpdateAsync(reginsDomainModel, id);
                if (reginsDomainModel == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<RegionDTO>(reginsDomainModel));

                // Convert Domain Mode to DTO
                //var regionDto = new RegionDTO()
                //{
                //    Id = reginsDomainModel.Id,
                //    Name = reginsDomainModel.Name,
                //    Code = reginsDomainModel.Code,
                //    RegionalImageUrl = reginsDomainModel.RegionalImageUrl
                //};
            //}
            //else
            //{
            //    return BadRequest(ModelState);
            //}

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var reginsDomain = await regionRepository.DeleteAsync(id);
            if (reginsDomain == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<RegionDTO>(reginsDomain));

            //var regDTO = new RegionDTO()
            //{
            //    Id = reginsDomain.Id,
            //    Name = reginsDomain.Name,
            //    Code = reginsDomain.Code,
            //    RegionalImageUrl = reginsDomain.RegionalImageUrl
            //};
        }
    }
}
