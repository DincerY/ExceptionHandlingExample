using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate requestDelegate)
    {
        _next = requestDelegate;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            Console.WriteLine("Exception middleware is running");
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }

    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;

        if (exception.GetType() == typeof(ValidationException))
        {
            return CreateValidationException(context, exception);
        }
        else if (exception.GetType() == typeof(AuthorizationException))
        {
            return CreateAuthorizationException(context, exception);
        }
        else if (exception.GetType() == typeof(BusinessException))
        {
            return CreateBusinessException(context,exception);
        }
        else
        {
            return CreateInternalException(context, exception);
        }
    }

  

    private Task CreateBusinessException(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

        BusinessProblemDetails problemDetails = new BusinessProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "type",
            Title = "Business Rule",
            Detail = "",
            Instance = "",
            Extensions = { new KeyValuePair<string, object>("1", 1) }
        };

        var reponse = JsonConvert.SerializeObject(problemDetails);
        return context.Response.WriteAsync(reponse);
    }


    private Task CreateValidationException(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

        ValidationProblemDetails problemDetails = new ValidationProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "type",
            Title = "Validation",
            Detail = "",
            Instance = "",
            Extensions = { new KeyValuePair<string, object>("1", 1) }
        };

        var response = JsonConvert.SerializeObject(problemDetails);

        return context.Response.WriteAsync(response);
    }


    private Task CreateAuthorizationException(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);

        AuthorizationProblemDetails problemDetails = new AuthorizationProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Type = "type",
            Title = "UnAuthorized",
            Detail = "",
            Instance = "",
            Extensions = { new KeyValuePair<string, object>("1", 1) }
        };

        var response = JsonConvert.SerializeObject(problemDetails);

        return context.Response.WriteAsync(response);
    }

    private Task CreateInternalException(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);

        ProblemDetails problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "type",
            Title = "Internal",
            Detail = "",
            Instance = "",
            Extensions = { new KeyValuePair<string, object>("1", 1) }
        };

        var response = JsonConvert.SerializeObject(problemDetails);

        return context.Response.WriteAsync(response);
    }
}