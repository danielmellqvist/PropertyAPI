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
            var realEstatesDto = _mapper.Map<IEnumerable<RealEstateDto>>(realEstates);
            return Ok(realEstatesDto);
        }

    }
}
