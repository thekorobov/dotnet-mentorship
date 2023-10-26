using System.Text;
using CodeSnippetRunner.CodeExecutorMicroservice.Models;
using CodeSnippetRunner.CodeExecutorMicroservice.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace CodeSnippetRunner.CodeExecutorMicroservice.Services;

public class RabbitMQListenerHostedService : IHostedService
{
    private readonly IRabbitMQResponseService _rabbitMqResponseService;
    private readonly IRabbitMQListenerService _rabbitMqListenerService;
    private readonly ICodeExecutionService _codeExecutionService;
    private readonly IBlobStorageService _blobStorageService;
    private readonly string _requestQueueName;

    public RabbitMQListenerHostedService(
        IOptions<RabbitMQConfiguration> options, 
        IRabbitMQListenerService rabbitMqListenerService, 
        IRabbitMQResponseService rabbitMqResponseService, 
        IBlobStorageService blobStorageService, 
        ICodeExecutionService codeExecutionService)
    {
        _rabbitMqListenerService = rabbitMqListenerService;
        _rabbitMqResponseService = rabbitMqResponseService;
        _blobStorageService = blobStorageService;
        _codeExecutionService = codeExecutionService;
        
        var rabbitMqOptions = options.Value;
        _requestQueueName = rabbitMqOptions.RequestQueueName;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _rabbitMqListenerService.StartListening(_requestQueueName, HandleMessageAsync);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task HandleMessageAsync(BasicDeliverEventArgs ea)
    {
        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
        var httpClientData = JsonConvert.DeserializeObject<HttpClientData>(message);

        var code = await _blobStorageService.ReadCodeAsync(httpClientData.BlobFileName, httpClientData.SasToken);
        var codeOutput = await _codeExecutionService.ExecuteCodeAsync(code);

        var replyTo = ea.BasicProperties.ReplyTo;
        var correlationId = ea.BasicProperties.CorrelationId;
        _rabbitMqResponseService.SendResponse(replyTo, correlationId, codeOutput);
    }
}