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
    [Route("api/users")]
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetInfoByUserName(string username)
        {
            var userReturned = await _repository.User.GetUserByUserNameAsync(username, trackChanges: false);
            int userId = userReturned.Id;
            var contactReturned = await _repository.Contact.GetContactByUserIdAsync(userId, trackChanges: false);
            int contactId = contactReturned.Id;
            var ratingsByUserId = await _repository.Rating.GetRatingsByUserIdAsync(userId, trackChanges: false);
            UserInformationDto userInformationDto = new()
            {
                UserName = username,
                RealEstates = (await _repository.RealEstate.GetAllRealEstatesByContactIdAsync(contactId, trackchanges: false)).Count(),
                Comments = (await _repository.Comment.GetAllCommentsByUserIdAsync(userId, trackChanges: false)).Count(),
                Rating = _repository.Rating.GetAverageRating(ratingsByUserId)

            };
            return Ok(userInformationDto);
        }

        [HttpPut("rate")]
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
