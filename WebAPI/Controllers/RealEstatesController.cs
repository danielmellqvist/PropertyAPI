using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
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
    //[ApiController]
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
        public async Task<IActionResult> GetAllRealEstates([FromQuery] RealEstateParameters realEstateParameters)
        {
            var realEstates = await _repository.RealEstate.GetAllRealEstatesAsync(realEstateParameters, trackChanges: false);
            if (realEstates == null)
            {
                _logger.LogInfo($"Could not find any Real Estates");
                return NotFound();
            }
            var realEstatesDto = _mapper.Map<IEnumerable<RealEstatesDto>>(realEstates);
            return Ok(realEstatesDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRealEstate([FromBody] RealEstateForCreationDto newRealEstate)
        {
            if (newRealEstate == null)
            {
                _logger.LogError("Client did not provide a new Real Estate object in request");
                return BadRequest("RealEstate object for creation is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the DTO in the CreateRealEstate action");
                return UnprocessableEntity(ModelState);
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

            var contact = await _repository.Contact.GetContactByTelephoneAsync(newRealEstate.Contact, trackChanges: false);
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
        public async Task<IActionResult> GetRealEstate(int id)
        {
            var realEstate = await _repository.RealEstate.GetRealEstateAsync(id, trackChanges: false);
            if (realEstate == null)
            {
                _logger.LogInfo($"Real Estate with the id : {id} doesn´t exist in the database.");
                return NotFound();
            }

            var realEstateDto = _mapper.Map<RealEstateDto>(realEstate);
            return Ok(realEstateDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRealEstate(int id)
        {
            var realEstate = await _repository.RealEstate.GetRealEstateAsync(id, trackChanges: true);
            if (realEstate == null)
            {
                _logger.LogInfo($"Real Estate with the id {id} did not exist");
                return NotFound();
            }
            _repository.RealEstate.DeleteRealEstate(realEstate);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
