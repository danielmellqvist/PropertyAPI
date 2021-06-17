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

        /// <summary> Retrieves all comments for a property with a given ID </summary>
        /// <remarks>
        /// 
        ///     GET /Comments
        ///     {
        ///         "Id": "1", 
        ///         "Skip": "2", 
        ///         "Take": 4", 
        ///     }
        /// </remarks>
        /// <response code="200">Returns a list of comments on a realestate </response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Could not find any comments</response>
        /// <response code="422">Unprocessable Entity</response>

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

        /// <summary> Retrieves all comments from a user with a given ID </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Comments
        ///         {
        ///             "Skip": "1", 
        ///             "Take": 2", 
        ///             "UserName: gasbagcur@mail.com"
        ///         }
        ///     
        /// </remarks>
        /// <response code="200">Returns a list of comments on a realestate </response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Could not find any comments</response>
        /// <response code="422">Unprocessable Entity</response>
        [HttpGet("ByUser/{userName}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetCommentsFromUserAsync([FromQuery] CommentsParameters commentsParameters, string userName)
        {
            _logger.LogInfo("Begin Search of User by UserName");
            var userTest = await _repository.User.GetUserByUserNameAsync(userName, trackChanges: false);
            if (userTest == null)
            {
                _logger.LogError("User does not exist");
                return NotFound("User does not exist");
            }
            var userId = userTest.Id;

            IEnumerable<Comment> comments = await  _repository.Comment.GetAllCommentsByUserIdWithParameters(commentsParameters, userId, trackChanges: false);

            if (comments.Any())
            {
                _logger.LogInfo("Begin Create User to return");
                var cleanedUserName = userName.Substring(0, userName.IndexOf("@"));
                List<CommentFromUserDto> commentFromUserDtos = new();
                foreach (var line in comments)
                {
                    CommentFromUserDto commentFromUserDto = new()
                    {
                        Content = line.Content,
                        UserName = cleanedUserName,
                        CreatedOn = line.CreatedOn
                        
                    };
                    commentFromUserDtos.Add(commentFromUserDto);
                }
                _logger.LogInfo("Creation of User to return completed");
                return Ok(commentFromUserDtos);
            }
            else
            {
                _logger.LogInfo($"User with UserName: {userName} does not exist in the Database");
                return NotFound($"User with UserName: {userName} does not exist in the Database");
            }
        }

        /// <summary> Creates a new comment. </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Comment
        ///     {
        ///         "Content": "This is a comment, great! ",
        ///         "RealEstateId": 10
        ///     }
        ///     
        /// </remarks>
        /// <returns>A newly created Comment</returns>
        /// <response code="201">Successfully created a comment</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Could not create comment</response>
        /// <response code="422">Unprocessable Entity</response>
        [HttpPost (Name= "NewCommentOnUser")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateComment([FromBody] CommentForCreationDto commentForCreationDto)
        {
            var realestateTest = _repository.RealEstate.GetRealEstateAsync(commentForCreationDto.RealEstateId, trackChanges: false);
            if (realestateTest.Result == null)
            {
                _logger.LogError("Real Estate does not exist");
                return NotFound("Real Estate does not exist");
            }

            commentForCreationDto.UserId = (await _repository.User.GetUserByUserNameAsync(HttpContext.User.Identity.Name.ToString(), trackChanges:false)).Id;
            commentForCreationDto.CreatedOn = DateTime.Now;
            _logger.LogInfo("Comment created and Mapping to be done");
            var commentCreated = _mapper.Map<Comment>(commentForCreationDto);
            _repository.Comment.CreateComment(commentCreated);
            await _repository.SaveAsync();

            var commentToReturn = _mapper.Map<CommentForReturnDto>(commentForCreationDto);
            commentToReturn.UserName = (HttpContext.User.Identity.Name.ToString()).Substring(0, (HttpContext.User.Identity.Name.ToString()).IndexOf("@"));

            return CreatedAtRoute("NewCommentOnUser", commentToReturn);
        }
    }
}

