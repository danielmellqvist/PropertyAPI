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
    [Route("api/comments")]
    [ApiController]
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

        [HttpGet("user/{id}")]
        public IActionResult GetCommentsFromUser([FromQuery] CommentsParameters commentsParameters, int id)
        {
            List<Comment> comments = _repository.Comment.GetAllCommentsByUserId(commentsParameters, id, trackChanges: false);
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

        [HttpGet("realestate/{id}")]
        public IActionResult GetCommentsForRealestate([FromQuery] CommentsParameters commentsParameters, int id)
        {
            List<Comment> estates = _repository.Comment.GetAllCommentsByRealEstateId(commentsParameters, id, trackChanges: false);
            if (estates.Count() != 0)
            {
                List<CommentsForRealEstateDto> commentsForRealEstateDtos = new();
                foreach (var estate in estates)
                {
                    var username = _context.Users.FirstOrDefault(x => x.Id == estate.UserId);
                    CommentsForRealEstateDto commentsForRealEstateDto = new CommentsForRealEstateDto
                    {
                        UserName = username.UserName,
                        Content = estate.Content,
                        CreatedOn = estate.CreatedOn
                    };
                    commentsForRealEstateDtos.Add(commentsForRealEstateDto);
                }
                return Ok(commentsForRealEstateDtos);
            }
            else
            {
                _logger.LogInfo($"Real Estate with id: {id} does not exist in the Database");
                return NotFound();
            }
        }


        //[HttpPost("{id}")]
        //public IActionResult CreateComment([FromBody] CommentForCreationDto commentForCreationDto, Guid id)
        //{
        //    commentForCreationDto.UserId = id;
        //    commentForCreationDto.CreatedOn = DateTime.Now;

        //    var commentCreated = _mapper.Map<Comment>(commentForCreationDto);


        //}


    }
}

