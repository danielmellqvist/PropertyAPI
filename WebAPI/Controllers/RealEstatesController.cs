using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Areas.Identity.Data;
using System.Text.RegularExpressions;
using WebAPI.ActionFilters;

namespace WebAPI.Controllers
{
    [Route("api/RealEstates")]
    public class RealEstatesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public RealEstatesController(ILoggerManager loggerManager, IRepositoryManager repositoryManager, IMapper mapper, UserManager<WebAPIUser> userManger)
        {
            _logger = loggerManager;
            _repository = repositoryManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a list of Real Estate listings.
        /// </summary>
        /// <remarks>
        /// Sample
        /// </remarks>
        /// <response code="200">Returns a list of Real Estates</response>
        /// <response code="404">Could not find any Real Estates</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidationRealEstatesExistsAttribute))]

        public IActionResult GetAllRealEstates([FromQuery] RealEstateParameters realEstateParameters)
        {
            var realEstates = HttpContext.Items["realEstateParameters"] as List<RealEstate>;
            var realEstatesDto = _mapper.Map<IEnumerable<RealEstatesDto>>(realEstates);
            return Ok(realEstatesDto);
        }

        [HttpPost]
        [Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateRealEstate([FromBody] RealEstateForCreationDto newRealEstate)
        {
            if (newRealEstate.RentingPrice == null && newRealEstate.SellingPrice == null)
            {
                _logger.LogError("At least one of the fields RentigPrice and SellingPrice must have values");
                return UnprocessableEntity("At least one of the fields RentigPrice and SellingPrice must have values");
            }
            var constructionYear = await _repository.ConstructionYear.GetFromYearAsync(newRealEstate.ConstructionYear, trackChanges: false);
            if (constructionYear == null)
            {
                _logger.LogInfo($"Construction year {newRealEstate.ConstructionYear} missing");
                constructionYear = new ConstructionYear() { Year = newRealEstate.ConstructionYear };
                _repository.ConstructionYear.CreateConstructionYear(constructionYear);
                await _repository.SaveAsync();
                _logger.LogInfo($"Construction year {constructionYear.Year} with the ID {constructionYear.Id} added to the database");
            }
            var cleanTelephone = Regex.Replace(newRealEstate.Contact, @"[^0-9+-]+", "");
            var contact = await _repository.Contact.GetContactByTelephoneAsync(cleanTelephone, trackChanges: false);
            if (contact == null)
            {
                _logger.LogInfo($"There were no contact with the phone number {newRealEstate.Contact} in the database");
                contact = new Contact() { Telephone = newRealEstate.Contact };
                _repository.Contact.CreateContact(contact);
                await _repository.SaveAsync();
                _logger.LogInfo($"Contact with the ID {contact.Id} added to the database");
            }
            var realEstateEntity = new RealEstate()
            {
                Title = newRealEstate.Title,
                Description = newRealEstate.Description,
                Street = newRealEstate.Address,
                ContactId = contact.Id,
                ConstructionYearId = constructionYear.Id,
                SellingPrice = (uint)newRealEstate.SellingPrice,
                CanBeSold = (newRealEstate.SellingPrice == null) ? false : true,
                RentingPrice = newRealEstate.RentingPrice,
                CanBeRented = (newRealEstate.RentingPrice == null) ? false : true,
                RealEstateTypeId = newRealEstate.Type,
                CreatedUtc = DateTime.UtcNow
            };
            await _repository.RealEstate.CreateRealEstateAsync(realEstateEntity);
            await _repository.SaveAsync();
            
            realEstateEntity = await _repository.RealEstate.GetRealEstateAsync(realEstateEntity.Id, trackChanges: false);
            var realEstateToReturn = _mapper.Map<RealEstateCreatedDto>(realEstateEntity);

            return CreatedAtRoute("RealEstateById", new { id = realEstateToReturn.Id}, realEstateToReturn);
        }



        [HttpGet("{id}", Name = "RealEstateById")]
        [ServiceFilter(typeof(ValidationSingleRealEstateExistsAttribute))]
        public async Task<IActionResult> GetRealEstate(int id)
        {
            var realEstate = HttpContext.Items["RealEstate"] as RealEstate;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var realEstatePrivate = _mapper.Map<RealEstatePrivateDto>(realEstate);
                var comments = await _repository.Comment.GetAllCommentsByRealEstateIdAsync(id: realEstate.Id, trackChanges: false);
                realEstatePrivate.Comments = _mapper.Map<IEnumerable<CommentsForRealEstateDto>>(comments);
                return Ok(realEstatePrivate);
            }
            else
            {
                var realEstatePublicDto = _mapper.Map<RealEstatePublicDto>(realEstate);
                return Ok(realEstatePublicDto);
            }
        }


        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidationSingleRealEstateExistsAttribute))]
        public async Task<IActionResult> DeleteRealEstate(int id)
        {
            var realEstate = HttpContext.Items["RealEstate"] as RealEstate;
            _repository.RealEstate.DeleteRealEstate(realEstate);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
