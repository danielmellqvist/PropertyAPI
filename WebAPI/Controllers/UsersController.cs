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

namespace WebAPI.Controllers
{
    [Route("api/users")]
    //[ApiController]
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

        [HttpGet("{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetInfoByUserName(string username)
        {
            if (username == null)
            {
                _logger.LogError("Username is null");
                return BadRequest("Username is null");
            }
            var userReturned = await _repository.User.GetUserByUserNameAsync(username, trackChanges: false);
            if (userReturned == null)
            {
                _logger.LogError($"User: {username} Does Not Exist");
                return BadRequest($"User: {username} Does Not Exist");
            }
            int userId = userReturned.Id;
            var contactReturned = await _repository.Contact.GetContactByUserIdAsync(userId, trackChanges: false);
            if (contactReturned == null)
            {
                _logger.LogError($"Contact Information about {username} does Not Exist");
                return BadRequest($"Contact Information about {username} does Not Exist");
            }
            int contactId = contactReturned.Id;
            var ratingsByUserId = await _repository.Rating.GetRatingsByUserIdAsync(userId, trackChanges: false);
            UserInformationDto userInformationDto = new()
            {
                UserName = username,
                RealEstates = (await _repository.RealEstate.GetAllRealEstatesByContactIdAsync(contactId, trackchanges: false)).Count(),
                Comments = (await _repository.Comment.GetAllCommentsByUserIdAsync(userId, trackChanges: false)).Count(),
                Rating = _repository.Rating.GetAverageRating(ratingsByUserId)

            };
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Model state for the User Object");
                return UnprocessableEntity(ModelState);
            }
            return Ok(userInformationDto);
        }

        [HttpPut("rate")]
        public async Task<IActionResult> CreateRatingForUser([FromBody] RatingAddNewRatingDto ratingAddNewRatingDto)
        {
            if (ratingAddNewRatingDto == null)
            {
                _logger.LogError("Rating Input object is null");
                return BadRequest("Rating Input object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Model state for the Rating Object");
                return UnprocessableEntity(ModelState);
            }
            var currentIdentityUser = await _repository.User.GetUserByUserNameAsync(HttpContext.User.Identity.Name.ToString(), trackChanges:false);
            if (currentIdentityUser == null)
            {
                _logger.LogError($"Information about {HttpContext.User.Identity.Name} does Not Exist");
                return BadRequest($"Information about {HttpContext.User.Identity.Name} does Not Exist");
            }
            var currentUser = await _repository.User.GetUserByGuidIdAsync(currentIdentityUser.IdentityUserId, trackChanges: false);
            if (currentUser == null)
            {
                _logger.LogError($"Information about Current User does Not Exist");
                return BadRequest($"Information about Current User does Not Exist");
            }
            var aboutUser = await _repository.User.GetUserByGuidIdAsync(ratingAddNewRatingDto.UserId, trackChanges: false);
            if (aboutUser == null)
            {
                _logger.LogError($"Information about User does Not Exist");
                return BadRequest($"Information about User does Not Exist");
            }
            ratingAddNewRatingDto.ByUserGuidId = currentUser.IdentityUserId;
            if (ratingAddNewRatingDto.UserId == ratingAddNewRatingDto.ByUserGuidId)
            {
                _logger.LogError($"User {currentUser.UserName} can not give themselves a rating NO SPAM ALLOWED!");
                return BadRequest($"User {currentUser.UserName} can not give themselves a rating NO SPAM ALLOWED!");
            }
            ratingAddNewRatingDto.ByUserId = currentUser.Id;
            ratingAddNewRatingDto.AboutUserId = aboutUser.Id;
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Model state for the RatingDto Object");
                return UnprocessableEntity(ModelState);
            }

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
