using System.Text;
using CodeSnippetRunner.Client.Models.Code;
using CodeSnippetRunner.Client.Models.Messaging;
using CodeSnippetRunner.Client.Services.Interfaces;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace CodeSnippetRunner.Client.Services;

public class RabbitMQService : IRabbitMQService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;
    private readonly string _requestQueueName;
    private readonly string _responseQueueName;

    public RabbitMQService(IOptions<RabbitMQConfiguration> options)
    {
        var rabbitMqOptions = options.Value;
        var factory = new ConnectionFactory()
        {
            UserName = rabbitMqOptions.Username,
            Password = rabbitMqOptions.Password,
            HostName = rabbitMqOptions.Hostname,
            VirtualHost = rabbitMqOptions.VirtualHost,
            Port = rabbitMqOptions.Port
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _consumer = new EventingBasicConsumer(_channel);
        _requestQueueName = rabbitMqOptions.RequestQueueName;
        _responseQueueName = rabbitMqOptions.ResponseQueueName;
    }
    
    public async Task<CodeOutputVm> SendAndReceive(object message)
    {
        var tcs = new TaskCompletionSource<CodeOutputVm>();

        var correlationId = Guid.NewGuid().ToString();
        
        var props = _channel.CreateBasicProperties();
        props.CorrelationId = correlationId;
        props.ReplyTo = _responseQueueName;

        var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        _channel.BasicPublish(
            exchange: "",
            routingKey: _requestQueueName,
            basicProperties: props,
            body: messageBytes);

        _consumer.Received -= ConsumerReceived!;  
        _consumer.Received += ConsumerReceived!;

        _channel.BasicConsume(
            consumer: _consumer,
            queue: _responseQueueName,
            autoAck: true);

        return await tcs.Task;

        void ConsumerReceived(object model, BasicDeliverEventArgs ea)
        {
            if (ea.BasicProperties.CorrelationId == correlationId)
            {
                var responseJson = Encoding.UTF8.GetString(ea.Body.ToArray());
                var response = JsonConvert.DeserializeObject<CodeOutputVm>(responseJson);
                tcs.SetResult(response);
            }
        }
    }
    
    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}