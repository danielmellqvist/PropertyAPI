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
        private readonly UserManager<WebAPIUser> _userManager;

        public CommentsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, PropertyContext context, UserManager<WebAPIUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }


        [HttpGet("{id}", Name = "CommentById")]
        public async Task<IActionResult> GetAllCommentsForRealEstate(int id, [FromQuery] CommentsParameters commentParam)
        {
            var comments = await _repository.Comment.GetAllCommentsByRealEstateIdParametersAsync(commentsParameter: commentParam, id, trackChanges: false);
            if (comments.Count() == 0)
            {
                _logger.LogInfo($"There were no comments for the real estate with id {id}.");
                return NotFound();
            }
            var commentsDto = _mapper.Map<IEnumerable<CommentsForRealEstateDto>>(comments);
            return Ok(commentsDto);
        }



        [HttpGet("user/{id}")]
        public IActionResult GetCommentsFromUser([FromQuery] CommentsParameters commentsParameters, int id)
        {
            List<Comment> comments = _repository.Comment.GetAllCommentsByUserIdWithParameters(commentsParameters, id, trackChanges: false);
            if (comments.Count() != 0)
            {
                var username = _context.Users.FirstOrDefault(x => x.Id == id);
                CommentFromUserDto commentFromUserDto = new()
                {
                    UserName = username.UserName
                };
                foreach (var line in comments)
                {
                    commentFromUserDto.Content.Add(line.Content);
                    commentFromUserDto.CreatedOn.Add(line.CreatedOn);
                }
                return Ok(commentFromUserDto);
            }
            else
            {
                _logger.LogInfo($"User with id: {id} does not exist in the Database");
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentForCreationDto commentForCreationDto)
        {
            var userName = HttpContext.User.Identity.Name;
            var user = await _repository.User.GetUserByUserNameAsync(userName, trackChanges: false);

            commentForCreationDto.UserId = user.Id;
            commentForCreationDto.CreatedOn = DateTime.Now;

            var commentCreated = _mapper.Map<Comment>(commentForCreationDto);
            _repository.Comment.CreateComment(commentCreated);
            await _repository.SaveAsync();

            var commentToReturn = _mapper.Map<CommentForReturnDto>(commentForCreationDto);
            commentToReturn.UserName = (await _repository.User.GetUserByUserId(user.Id, trackChanges: false)).UserName;

            return Ok(commentToReturn);
        }
    }
}

