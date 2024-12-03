using System.Net;
using Contracts;
using Entities.ErrorModels;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CompanyEmployees.Extensions;

public static class ExceptionMiddlewareExtensions
{
  public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger)
  {
    app.UseExceptionHandler(appError =>
    {
      appError.Run(async context =>
      {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";
        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
          logger.LogError($"Something went wrong: {contextFeature.Error}");
          context.Response.StatusCode = contextFeature.Error switch
          {
            NotFoundException => StatusCodes.Status404NotFound,
            BadRequestException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
          };

          await context.Response.WriteAsync(new ErrorDetails()
          {
            StatusCode = context.Response.StatusCode,
            Message = "Internal Server Error.",
          }.ToString());
        }
      });
    });
  }
}
