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
    public class ValidationSingleRealEstateExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public ValidationSingleRealEstateExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            bool trackChanges = (method.Equals("DELETE") || method.Equals("GET")) ? true : false;

            var realEstateId = (int)context.ActionArguments["id"];
            var foundRealestates = await _repository.RealEstate.GetRealEstateAsync(realEstateId, trackChanges);
            if (foundRealestates == null)
            {
                _logger.LogInfo($"Real Estate with the id : {realEstateId} doesn´t exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("RealEstate", foundRealestates);
                await next();
            }
        }
    }
}
