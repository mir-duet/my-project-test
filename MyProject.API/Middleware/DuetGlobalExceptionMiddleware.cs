using MyProject.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace MyProject.API.Middleware;

public class DuetGlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DuetGlobalExceptionMiddleware> _logger;

    public DuetGlobalExceptionMiddleware(RequestDelegate next, ILogger<DuetGlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "An internal server error occurred.";

        switch (exception)
        {
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = notFoundException.Message;
                break;
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                message = validationException.Message;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = message
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}
