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
    public class ValidationRealEstatesExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public ValidationRealEstatesExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            bool trackChanges = (method.Equals("PUT") || method.Equals("GET")) ? true : false;
            RealEstateParameters realEstateParameter = (RealEstateParameters)context.ActionArguments["realEstateParameters"];
            var foundRealestates = await _repository.RealEstate.GetAllRealEstatesAsync(realEstateParameter, trackChanges);
            if (foundRealestates == null)
            {
                _logger.LogInfo($"Could not find any Real Estates");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("realEstateParameters", foundRealestates);
                await next();
            }
        }
    }
}
