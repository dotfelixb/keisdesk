﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Swats.Model.ViewModel;

namespace Swats.Infrastructure;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            var errorResult = new ErrorResult
            {
                Ok = false,
                Errors = errors
            };

            context.Result = new BadRequestObjectResult(errorResult);
            return;
        }

        await next();
    }
}
