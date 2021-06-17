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
        /// Returns a list of RealEstate listings.
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


        /// <summary>
        /// Create one RealEstate
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /RealEstates
        ///     {
        ///         "Title": "Some very interesting office", 
        ///         "Description": "You will love it. The view is great!", 
        ///         "Address": "Mladost 1A, Telerik Academy building", 
        ///         "Contact": "0888-888-888", 
        ///         "ConstructionYear": 2005, 
        ///         "SellingPrice": 120000, 
        ///         "RentingPrice": null, 
        ///         "Type": 2 
        ///     }
        ///     
        /// </remarks>
        /// <returns>A newly created RealEstate</returns>
        /// <response code="201">Returns the newly created Real Estate</response>
        /// <response code="400">The object in the request is null</response>
        /// <response code="422">Real Estate object in the request is un processable</response>
        [HttpPost]
        [Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
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


        /// <summary>
        /// Get one RealEstate by ID
        /// 
        /// If the user is logged in some additional information like the contact information and comments are sent.
        /// </summary>
        /// <remarks> 
        /// </remarks>
        /// <returns>One RealEstate by Id</returns>
        /// <response code="200">Returns the Real Estate</response>
        /// <response code="404">The object could not be found</response>
        [HttpGet("{id}", Name = "RealEstateById")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Delete a RealEstate from the database by Id
        /// </summary>
        /// <param name="id">Database Id of the RealEstate</param>
        /// <response code="204">Post deleted</response>
        /// /// <response code="404">The object could not be found</response>
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidationSingleRealEstateExistsAttribute))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRealEstate(int id)
        {
            var realEstate = HttpContext.Items["RealEstate"] as RealEstate;
            _repository.RealEstate.DeleteRealEstate(realEstate);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
