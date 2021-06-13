﻿using AutoMapper;
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
    [Route("api/Comments")]
    //[ApiController]
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


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllCommentsForRealEstate(int id, [FromQuery] CommentsParameters commentParameters)
        {
            var comments = await _repository.Comment.GetAllCommentsByRealEstateIdParametersAsync(commentParameters, id, trackChanges: false);
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


        [HttpPost("Create/{id}", Name = "CommentById")]
        public async Task<IActionResult> CreateComment([FromBody] CommentForCreationDto commentForCreationDto, int id)
        {
            commentForCreationDto.UserId = id;
            commentForCreationDto.CreatedOn = DateTime.Now;

            var commentCreated = _mapper.Map<Comment>(commentForCreationDto);
            _repository.Comment.CreateComment(commentCreated);
            await _repository.SaveAsync();

            var commentToReturn = _mapper.Map<CommentForReturnDto>(commentForCreationDto);
            commentToReturn.UserName = (await _repository.User.GetUserName(id, trackchanges: false)).UserName;

            return Ok(commentToReturn);
        }
    }
}

