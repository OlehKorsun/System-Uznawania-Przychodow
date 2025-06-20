using System.Net;
using System_Uznawania_Przychodow.Exceptions;

namespace System_Uznawania_Przychodow.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Call the next middleware in the pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "An unhandled exception occurred");

            // Handle the exception
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode;
        string message;

        switch (exception)
        {
            case KeyNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                message = exception.Message;
                break;
            case UserExistsException:
                statusCode = (int)HttpStatusCode.Conflict;
                message = exception.Message;
                break;
            case UpdateClientException:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
            case ClientHasExistsException:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
            case DateException:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
            case ClientHasContractException:
                statusCode = (int)HttpStatusCode.Conflict;
                message = exception.Message;
                break;
            case UnauthorizedAccessException:
                statusCode = (int)HttpStatusCode.Unauthorized;
                message = "Brak dostępu";
                break;
            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                message = "Wewnętrzny błąd serwera.";
                break;
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = new
            {
                message,
                type = exception.GetType().Name
            }
        };

        var json = System.Text.Json.JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}