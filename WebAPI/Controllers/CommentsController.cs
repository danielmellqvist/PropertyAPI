using AutoMapper;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
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
    [Route("api/[controller]")]
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

        [HttpGet("{id}")]
        public IActionResult GetCommentsFromUser(Guid id)
        {
            List<Comment> comments = _repository.Comment.GetAllCommentsByUserId(id, trackChanges: false);
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
            
                
            
            //var commentDto = _mapper.Map<IEnumerable<CommentFromUserDto>>(comments);
            // TODO FIX
            return Ok(comments);
    }
}
}
