using AutoMapper;
using Entities;
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
using WebAPI.ActionFilters;
using WebAPI.Areas.Identity.Data;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/Comments")]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CommentsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }



        /// <summary>
        /// Retrieves all comments for a property with a given ID
        /// </summary>
        /// <response code="200">Returns a list of comments on a realestate </response>
        /// <response code="404">Could not find any comments</response>

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidationGettingCommentsForRealEstateAttribute))]
        public IActionResult GetAllCommentsForRealEstate(int id, [FromQuery] CommentsParameters commentParam)
        {
            var comments = HttpContext.Items["foundCommentsRealestates"] as List<Comment>;
            var commentsDto = _mapper.Map<IEnumerable<CommentsForRealEstateDto>>(comments);
            _logger.LogInfo($"Begin trimming of usernames");
            foreach (var comment in commentsDto)
            {
                comment.UserName = comment.UserName.Substring(0, comment.UserName.IndexOf("@"));
            }
            _logger.LogInfo($"Succesfully Mapped Realestates");
            return Ok(commentsDto);
        }

        /// <summary>
        /// Retrieves all comments written by the user with the specified username.
        /// </summary>
        /// <response code="200">Returns a list of comments on a user </response>
        /// <response code="404">Could not find any comments</response>
        [HttpGet("ByUser/{userName}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetCommentsFromUserAsync([FromQuery] CommentsParameters commentsParameters, string userName)
        {
            if (commentsParameters == null || userName == null)
            {
                _logger.LogError("Comment Input or username is null");
                return BadRequest("Comment Input or username is null");
            }
            _logger.LogInfo("Begin Search of User by UserName");
            var userId = (await _repository.User.GetUserByUserNameAsync(userName, trackChanges: false)).Id;
            IEnumerable<Comment> comments = await  _repository.Comment.GetAllCommentsByUserIdWithParameters(commentsParameters, userId, trackChanges: false);
            if (comments.Any())
            {
                _logger.LogInfo("Begin Create User to return");
                var cleanedUserName = userName.Substring(0, userName.IndexOf("@"));
                CommentFromUserDto commentFromUserDto = new()
                {
                    UserName = cleanedUserName
                };
                foreach (var line in comments)
                {
                    commentFromUserDto.Content.Add(line.Content);
                    commentFromUserDto.CreatedOn.Add(line.CreatedOn);
                }
                _logger.LogInfo("Creation of User to return completed");
                return Ok(commentFromUserDto);
            }
            else
            {
                _logger.LogInfo($"User with UserName: {userName} does not exist in the Database");
                return NotFound($"User with UserName: {userName} does not exist in the Database");
            }
        }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <response code="202">Successfully created a comment </response>
        /// <response code="404">Could not create comment </response>
        [HttpPost("{id}", Name = "CommentById")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateComment([FromBody] CommentForCreationDto commentForCreationDto, int id)
        {
            if (commentForCreationDto == null || id == 0)
            {
                _logger.LogError("Comment Input or Id is null");
                return BadRequest("Comment Input or Id is null");
            }
            commentForCreationDto.UserId = id;
            commentForCreationDto.CreatedOn = DateTime.Now;
            _logger.LogInfo("Comment created and Mapping to be done");
            var commentCreated = _mapper.Map<Comment>(commentForCreationDto);
            _repository.Comment.CreateComment(commentCreated);
            await _repository.SaveAsync();

            var commentToReturn = _mapper.Map<CommentForReturnDto>(commentForCreationDto);
            commentToReturn.UserName = (await _repository.User.GetUserByUserIdAsync(id, trackChanges: false)).UserName;

            return Ok(commentToReturn);
        }
    }
}

