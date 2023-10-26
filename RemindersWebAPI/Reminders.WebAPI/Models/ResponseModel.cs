using System.Net;

namespace Reminders.WebAPI.Models;

[Serializable]
public class ResponseModel
{
    public string? RequestUrl { get; set; }
    public object? Data { get; set; }
    public string? Error { get; set; }
    public bool ResultStatus { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}