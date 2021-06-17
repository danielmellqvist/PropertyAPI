using AutoMapper;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using System.Web;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Areas.Identity.Data;
using WebAPI.ActionFilters;

namespace WebAPI.Controllers
{
    [Route("api/Users")]
    [Authorize]
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

        /// <summary>
        /// Retrieves information about the user chosen by their Username.
        /// </summary>
        /// <response code="200">Successfully shows user information in list of amounts of different properties</response>
        /// <response code="404">Could not find a user</response>
        [HttpGet("{username}")]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetInfoByUserName(string username)
        {
            var userReturned = await _repository.User.GetUserByUserNameAsync(username, trackChanges: false);
            if (userReturned == null)
            {
                _logger.LogError("User does not exist");
                return NotFound("User Not found");
                 
            }
            var contactReturned = await _repository.Contact.GetContactByUserIdAsync(userReturned.Id, trackChanges: false);
            if (contactReturned == null)
            {
                _logger.LogError("Contact for the user does not exist");
                return NotFound("Contact for the user does not exist");

            }
            var ratingsByUserId = await _repository.Rating.GetRatingsByUserIdAsync(userReturned.Id, trackChanges: false);
            string rating = _repository.Rating.GetAverageRating(ratingsByUserId).ToString();
            if (rating == "0")
            {
                rating = "No Ratings Given Yet";
            }
            UserInformationDto userInformationDto = new()
            {
                UserName = username,
                RealEstates = (await _repository.RealEstate.GetAllRealEstatesByContactIdAsync(contactReturned.Id, trackchanges: false)).Count(),
                Comments = (await _repository.Comment.GetAllCommentsByUserIdAsync(userReturned.Id, trackChanges: false)).Count(),
                Rating = rating

            };
            return Ok(userInformationDto);
        }

        /// <summary>
        /// Creates a new Rating from a user about a user, checking for spam.
        /// </summary>
        /// <response code="200">Successfully created a new rating</response>
        /// <response code="404">Could not create a new rating</response>
        [HttpPut("Rate")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateRatingForUser([FromBody] RatingAddNewRatingDto ratingAddNewRatingDto)
        {
            var currentIdentityUser = await _repository.User.GetUserByUserNameAsync(HttpContext.User.Identity.Name.ToString(), trackChanges: false);
            if (currentIdentityUser == null)
            {
                _logger.LogError("Current user information does not exist");
                return NotFound("Current user information does not exist");
            }
            var currentUser = await _repository.User.GetUserByGuidIdAsync(currentIdentityUser.IdentityUserId, trackChanges: false);
            if (currentUser == null)
            {
                _logger.LogError("Current user information does not exist");
                return NotFound("Current user information does not exist");
            }
            var aboutUser = await _repository.User.GetUserByGuidIdAsync(ratingAddNewRatingDto.UserId, trackChanges: false);
            if (aboutUser == null)
            {
                _logger.LogError("Searched user information does not exist");
                return NotFound("Searched  user information does not exist");
            }
            ratingAddNewRatingDto.ByUserGuidId = currentUser.IdentityUserId;
            if (ratingAddNewRatingDto.UserId == ratingAddNewRatingDto.ByUserGuidId)
            {
                _logger.LogError($"User {currentUser.UserName} can not give themselves a rating NO SPAM ALLOWED!");
                return BadRequest($"User {currentUser.UserName} can not give themselves a rating NO SPAM ALLOWED!");
            }
            ratingAddNewRatingDto.ByUserId = currentUser.Id;
            ratingAddNewRatingDto.AboutUserId = aboutUser.Id;
            bool checkUser = await _repository.Rating.CheckMultipleRatingsFromUserAsync(ratingAddNewRatingDto);
            if (!checkUser)
            {
                var ratingCreated = _mapper.Map<Rating>(ratingAddNewRatingDto);
                await _repository.Rating.CreateNewRating(ratingCreated);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                _logger.LogError($"User {currentUser.UserName} has already posted a rating about User {aboutUser.UserName}. NO SPAM ALLOWED!");
                return BadRequest($"User {currentUser.UserName} has already posted a rating about User {aboutUser.UserName}. NO SPAM ALLOWED!");
            }
        }
    }
}
