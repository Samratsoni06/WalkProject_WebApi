using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WalkProject_WebApi.Data;
using WalkProject_WebApi.Models.Domain;
using WalkProject_WebApi.Models.DTO;
using WalkProject_WebApi.Repository;

namespace WalkProject_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkController : ControllerBase
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        public WalkController(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] AddWalkRequestDTO addWalkRequestDTO)
        {
                var WalkModelDom = _mapper.Map<Walk>(addWalkRequestDTO);
                await _walkRepository.CreateAsync(WalkModelDom);
                return Ok(_mapper.Map<WalkDTO>(WalkModelDom));          
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filteron = null, [FromQuery] string? filterquery = null, [FromQuery] string? SortBy = null, [FromQuery] bool? isAcending = true, [FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 1000)
        {
            //var walkDom = await _walkRepository.GetWalkAsync();
            var walkDom = await _walkRepository.GetWalkAsync(filteron,filterquery, SortBy, isAcending ?? true,PageNumber,PageSize);
            return Ok(_mapper.Map<List<WalkDTO>>(walkDom));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _walkRepository.GetWalkByIdAsync(id);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<WalkDTO>(result));
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkDTO updateWalkDTO )
        {
                var walkDomainModel = _mapper.Map<Walk>(updateWalkDTO);
                walkDomainModel = await _walkRepository.UpdateAsync(walkDomainModel, id);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<WalkDTO>(walkDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDom = await _walkRepository.DeleteAsync(id);
            if (walkDom == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<WalkDTO>(walkDom));
        }
    }
}
