using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
                var reginsDomain = await regionRepository.GetAllAsync();
                return Ok(mapper.Map<List<RegionDTO>>(reginsDomain));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }

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
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }

        }

        [HttpPost]
        [ValidationModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto Dto)
        {
            var regionDmainModel = mapper.Map<Region>(Dto);

            regionDmainModel = await regionRepository.CreateAsync(regionDmainModel);

            var regionDto = mapper.Map<Region>(Dto);

            return CreatedAtAction(nameof(GetById), new { id = regionDmainModel.Id }, regionDmainModel);            
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var reginsDomainModel = mapper.Map<Region>(updateRegionRequestDto);          
            reginsDomainModel = await regionRepository.UpdateAsync(reginsDomainModel, id);
            if (reginsDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<RegionDTO>(reginsDomainModel));
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
        }
    }
}
