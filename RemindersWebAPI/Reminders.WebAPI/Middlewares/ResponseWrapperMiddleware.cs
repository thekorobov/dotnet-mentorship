using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using Reminders.WebAPI.Models;

namespace Reminders.WebAPI.Middlewares;

public class ResponseWrapperMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseWrapperMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var currentBody = context.Response.Body;

        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        await _next(context);

        context.Response.Body = currentBody;

        memoryStream.Seek(0, SeekOrigin.Begin);

        var readToEnd = await new StreamReader(memoryStream).ReadToEndAsync();

        object result = null;
        
        if (!string.IsNullOrEmpty(readToEnd))
        {
            try
            {
                if (readToEnd.StartsWith("{") && readToEnd.EndsWith("}") || 
                    readToEnd.StartsWith("[") && readToEnd.EndsWith("]"))
                {
                    result = JsonConvert.DeserializeObject(readToEnd);
                }
                else
                {
                    result = readToEnd; 
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        var response = ResponseWrapper(result, context);

        if (context.Response.StatusCode != StatusCodes.Status204NoContent)
        {
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
    
    private static ResponseModel ResponseWrapper(object? result, HttpContext context, object? exception = null)
    {
        var requestUrl = context.Request.GetDisplayUrl();
        var data = result;
        var error = exception != null ? "ResponseWrapperMiddleware exception" : null;
        var resultStatus = result != null;
        var httpStatusCode = (HttpStatusCode)context.Response.StatusCode;

        return new ResponseModel()
        {
            RequestUrl = requestUrl,
            Data = data,
            Error = error,
            ResultStatus = resultStatus,
            StatusCode = httpStatusCode
        };
    }
}