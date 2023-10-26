using System.Net;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Application.Common.Exceptions.Halls;
using TicketFlow.Application.Common.Exceptions.Seats;
using TicketFlow.Application.Common.Exceptions.Users;
using JsonSerializer = System.Text.Json.JsonSerializer;
using ValidationException = FluentValidation.ValidationException;

namespace TicketFlow.WebApi.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(httpContext, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        _logger.LogError(exception.Message);
        
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;
        
        switch(exception)
        {
            case ValidationException:
            case CapacityExceededException:
            case InvalidSeatConfigurationException:
                code = HttpStatusCode.BadRequest;
                break;
            case EntityNotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case UnauthorizedAccessException:
            case PermissionDeniedException:
                code = HttpStatusCode.Forbidden;
                break;
            case UserAlreadyExistsException:
            case SeatAlreadyExistsException:
                code = HttpStatusCode.Conflict;
                break;
            case UserCreationFailedException:
            case RoleAssignmentFailedException:
            case PasswordResetFailedException:
                code = HttpStatusCode.InternalServerError;
                break;
        }
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)code;
        
        if (result == string.Empty)
        {
            result = JsonSerializer.Serialize(new { error = exception.Message });
        }

        await httpContext.Response.WriteAsync(result);
    }
}