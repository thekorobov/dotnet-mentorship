using System.Net;
using Reminders.BLL.DTO.Exceptions;
using JsonSerializer = System.Text.Json.JsonSerializer;
using ValidationException = FluentValidation.ValidationException;

namespace Reminders.WebAPI.Middlewares;

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
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(validationException.Errors);
                break;
            case EntityNotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case UnauthorizedAccessException:
            case UserNotVerifiedException:
                code = HttpStatusCode.Forbidden;
                break;
            case UserAlreadyExistsException:
                code = HttpStatusCode.Conflict;
                break;
            case UserCreationFailedException:
            case RoleAssignmentFailedException:
            case PasswordResetFailedException:
                code = HttpStatusCode.InternalServerError;
                break;
            case UserAlreadyVerifiedException:
                code = HttpStatusCode.OK;
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