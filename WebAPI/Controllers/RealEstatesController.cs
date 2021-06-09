using AutoMapper;
using Entities.DataTransferObjects;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/RealEstates")]
    [ApiController]
    public class RealEstatesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public RealEstatesController(ILoggerManager loggerManager, IRepositoryManager repositoryManager, IMapper mapper)
        {
            _logger = loggerManager;
            _repository = repositoryManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllRealEstates()
        {
            var realEstates = _repository.RealEstate.GetAllRealEstates(trackChanges: false);
            var realEstatesDto = _mapper.Map<IEnumerable<RealEstatesDto>>(realEstates);
            return Ok(realEstatesDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetRealEstate(int id)
        {
            var realEstate = _repository.RealEstate.GetRealEstate(id, trackChanges: false);
            if (realEstate == null)
            {
                _logger.LogInfo($"Real Estate with the id : {id} doesn´t exist in the database.");
                return NotFound();
            }
            else
            {
                var realEstateDto = _mapper.Map<RealEstateDto>(realEstate);
                realEstateDto.ConstructionYear = _repository.ConstructionYear.GetYearFromId(realEstate.ConstructionYearId, trackChanges: false);
                return Ok(realEstateDto);
            }
        }
    }
}
