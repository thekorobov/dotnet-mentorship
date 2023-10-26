using System.Net;

namespace TicketFlow.WebApi.Models;

[Serializable]
public class ResponseVm
{
    public string? RequestUrl { get; set; }
    public object? Data { get; set; }
    public string? Error { get; set; }
    public bool ResultStatus { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}