using LoggerService.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        private readonly ILoggerManager _logger;

        public ValidationFilterAttribute(ILoggerManager logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];

            var param = context.ActionArguments.SingleOrDefault(x => x.Value.ToString().Any()).Value;   // Here we check if anything is created
            if (param == null)
            {
                _logger.LogError($"Object sent from client is null. Controller: {controller}, action: { action} "); 
                context.Result = new BadRequestObjectResult($"Object is null. Controller: { controller }, action: { action} "); 
                return;
            }

            if (!context.ModelState.IsValid)        // if nothing was created we can check the model validity
            {
                _logger.LogError($"Invalid model state for the object. Controller: {controller}, action: { action} "); 
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }


    }
}
