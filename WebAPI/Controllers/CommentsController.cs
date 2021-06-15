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
using WebAPI.Areas.Identity.Data;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/Comments")]
    //[ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly PropertyContext _context;
        public CommentsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, PropertyContext context)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        // Done
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllCommentsForRealEstate(int id, [FromQuery] CommentsParameters commentParam)
        {
            if (commentParam == null || id == 0)
            {
                _logger.LogError("Comment Input object or Id is null");
                return BadRequest("Comment Input object or Id is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Model state for the Comment Object");
                return UnprocessableEntity(ModelState);
            }
            _logger.LogInfo($"Retrieving Comments for RealEstateId: {id}");
            var comments = await _repository.Comment.GetAllCommentsByRealEstateIdParametersAsync(commentsParameter: commentParam, id, trackChanges: false);
            if (comments.Count() == 0)
            {
                _logger.LogInfo($"There were no comments for the real estate with id {id}.");
                return NotFound($"There were no comments for the real estate with id {id}.");
            }
            _logger.LogInfo($"Succesfully Retrieved all comments for RealEstateId: {id}");
            var commentsDto = _mapper.Map<IEnumerable<CommentsForRealEstateDto>>(comments);
            _logger.LogInfo($"Begin trimming of usernames");
            foreach (var comment in commentsDto)
            {
                comment.UserName = comment.UserName.Substring(0, comment.UserName.IndexOf("@"));
            }
            _logger.LogInfo($"Succesfully Mapped Realestates");
            return Ok(commentsDto);
        }


        // Done
        [HttpGet("ByUser/{userName}")]
        public async Task<IActionResult> GetCommentsFromUserAsync([FromQuery] CommentsParameters commentsParameters, string userName)
        {
            if (commentsParameters == null || userName == null)
            {
                _logger.LogError("Comment Input or username is null");
                return BadRequest("Comment Input or username is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Model state for the Comment Object");
                return UnprocessableEntity(ModelState);
            }
            _logger.LogInfo("Begin Search of User by UserName");
            var userId = (await _repository.User.GetUserByUserNameAsync(userName, trackChanges: false)).Id;
            IEnumerable<Comment> comments = await  _repository.Comment.GetAllCommentsByUserIdWithParameters(commentsParameters, userId, trackChanges: false);
            if (comments.Count() != 0)
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

        // Done
        [HttpPost("Create/{id}", Name = "CommentById")]
        public async Task<IActionResult> CreateComment([FromBody] CommentForCreationDto commentForCreationDto, int id)
        {
            if (commentForCreationDto == null || id == 0)
            {
                _logger.LogError("Comment Input or Id is null");
                return BadRequest("Comment Input or Id is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Model state for the Comment Object");
                return UnprocessableEntity(ModelState);
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

