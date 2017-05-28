using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebManager.Filters
{
    public class ValidateFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState
                    .Values
                    .Where(m => m.Errors.Any())
                    .Select(m => string.Join(",", m.Errors.Select(e => e.ErrorMessage)));

                if (actionContext.HttpContext.IsApiUrl())
                {
                    actionContext.Result = new BadRequestObjectResult(errors);
                }
                else
                {
                    actionContext.Result = new ViewResult();
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
