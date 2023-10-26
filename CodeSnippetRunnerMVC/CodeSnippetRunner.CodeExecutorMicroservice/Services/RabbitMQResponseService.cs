using System.Text;
using CodeSnippetRunner.CodeExecutorMicroservice.Models;
using CodeSnippetRunner.CodeExecutorMicroservice.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CodeSnippetRunner.CodeExecutorMicroservice.Services;

public class RabbitMQResponseService : IRabbitMQResponseService, IDisposable
{
    private readonly IModel _channel;

    public RabbitMQResponseService(IConnection connection)
    {
        _channel = connection.CreateModel();
    }

    public void SendResponse(string replyTo, string correlationId, CodeOutput codeOutput)
    {
        var props = _channel.CreateBasicProperties();
        props.CorrelationId = correlationId;

        var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(codeOutput));
        _channel.BasicPublish(
            exchange: "",
            routingKey: replyTo,
            basicProperties: props,
            body: responseBytes);
    }
    
    public void Dispose()
    {
        _channel?.Dispose();
    }
}