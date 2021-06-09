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

        public RealEstatesController(ILoggerManager loggerManager, IRepositoryManager repositoryManager)
        {
            _logger = loggerManager;
            _repository = repositoryManager;
        }

        [HttpGet]
        public IActionResult GetAllRealEstates()
        {
            try
            {
                var realEstates = _repository.RealEstate.GetAllRealEstates(trackChanges: false);
                return Ok(realEstates);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured in the {nameof(GetAllRealEstates)} action. {ex}");
                return StatusCode(500, "Internal server error");
                throw;
            }
            
        }

    }
}
