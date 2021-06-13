using AutoMapper;
using Entities;
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
    [Route("api/users")]
    //[ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly PropertyContext _context;

        public UsersController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, PropertyContext context)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetInfoByUserName(string username)
        {
            int userId = (await _repository.User.GetUserByUserNameAsync(username, trackChanges: false)).Id;
            int contactId = (await _repository.Contact.GetContactByUserId(userId, trackChanges: false)).Id;
            var ratingsByUserId = await _repository.Rating.GetRatingsByUserId(userId, trackChanges: false);
            UserInformationDto userInformationDto = new UserInformationDto
            {
                UserName = username,
                Realestates = (await _repository.RealEstate.GetAllRealEstatesByContactId(contactId, trackchanges: false)).Count(),
                Comments = (await _repository.Comment.GetAllCommentsByUserId(userId, trackChanges: false)).Count(),
                Rating = _repository.Rating.GetAverageRating(ratingsByUserId)

            };
            return Ok(userInformationDto);
        }

        //[HttpPut("rate")]
        //public async Task<IActionResult> CreateRatingForUser([FromBody] RatingAddNewRatingDto ratingAddNewRatingDto)
        //{
            //ratingAddNewRatingDto.ByUserId == HttpContext.User.Identity.Name;
            //bool checkUser = await _repository.Rating.CheckMultipleRatingsFromUser(ratingAddNewRatingDto);
        //    if (checkUser)
        //    {
        //        var ratingCreated = _mapper.Map<Rating>(ratingAddNewRatingDto);
        //        //await _repository.Rating.CreateNewRating(ratingCreated);
        //        await _context.SaveChangesAsync();
        //        return NoContent();
        //    }
        //    else
        //    {
        //        _logger.LogError($"User has already posted a rating about User {ratingAddNewRatingDto.UserId}. NO SPAM ALLOWED!");
        //        return BadRequest("Dont Spam User");
        //    }
        //}
    }
}
