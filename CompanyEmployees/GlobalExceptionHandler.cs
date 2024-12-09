﻿using System.Net;
using Contracts;
using Entities.ErrorModels;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CompanyEmployees
{
  public class GlobalExceptionHandler(ILoggerManager logger) : IExceptionHandler
  {
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
      httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      httpContext.Response.ContentType = "application/json";

      var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
      if (contextFeature != null)
      {
        httpContext.Response.StatusCode = contextFeature.Error switch
        {
          NotFoundException => StatusCodes.Status404NotFound,
          BadRequestException => StatusCodes.Status400BadRequest,
          _ => StatusCodes.Status500InternalServerError
        };
        logger.LogError($"Something went wrong: {exception.Message}");

        await httpContext.Response.WriteAsync(new ErrorDetails()
        {
          StatusCode = httpContext.Response.StatusCode,
          Message = "Internal Server Error.",
        }.ToString());
      }

      return true;
    }
  }
}
