using Entities.RequestFeatures;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.ActionFilters
{
    public class ValidationGettingCommentsForRealEstateAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public ValidationGettingCommentsForRealEstateAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var realEstateId = (int)context.ActionArguments["id"];
            CommentsParameters commentsParameters = (CommentsParameters)context.ActionArguments["commentParam"];
            var foundCommentsRealestates = await _repository.Comment.GetAllCommentsByRealEstateIdParametersAsync( commentsParameters, realEstateId, trackChanges: false);
            if (!foundCommentsRealestates.Any())
            {
                _logger.LogInfo($"There were no comments for the real estate with id {realEstateId}.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("foundCommentsRealestates", foundCommentsRealestates);
                await next();
            }
        }
    }
}
