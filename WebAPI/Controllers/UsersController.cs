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

        // marcus added
        private readonly IdentityContext _identityContext;

        public UsersController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, PropertyContext context, IdentityContext identityContext)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _context = context;
            _identityContext = identityContext;
        }

        [HttpGet("{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetInfoByUserName(string username)
        {
            int userId = (await _repository.User.GetUserByUserName(username, trackChanges: false)).Id;
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

        [HttpPut("rate")]
        public async Task<IActionResult> CreateRatingForUser([FromBody] RatingAddNewRatingDto ratingAddNewRatingDto)
        {
            var currentIdentityUser = _identityContext.Users.FirstOrDefault(x => x.UserName == HttpContext.User.Identity.Name.ToString());
            var currentUser = (await _repository.User.GetUserByGuidId(Guid.Parse(currentIdentityUser.Id), trackChanges: false));
            var aboutUser = (await _repository.User.GetUserByGuidId(ratingAddNewRatingDto.UserGuidId, trackChanges: false));

            ratingAddNewRatingDto.ByUserGuidId = currentUser.IdentityUserId;
            ratingAddNewRatingDto.ByUserId = currentUser.Id;
            ratingAddNewRatingDto.AboutUserId = aboutUser.Id;
            

            bool checkUser = await _repository.Rating.CheckMultipleRatingsFromUser(ratingAddNewRatingDto);
            if (checkUser)
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
