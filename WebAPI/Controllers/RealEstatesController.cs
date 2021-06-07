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
    [Route("api/[controller]")]
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

        //[HttpGet]
        //public IActionResult GetRealEstates()
        //{
        //    //_repository.Comment.
        //}

    }
}
