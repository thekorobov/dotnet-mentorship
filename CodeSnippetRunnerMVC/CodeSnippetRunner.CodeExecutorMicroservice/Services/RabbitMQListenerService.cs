using CodeSnippetRunner.CodeExecutorMicroservice.Models;
using CodeSnippetRunner.CodeExecutorMicroservice.Services.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CodeSnippetRunner.CodeExecutorMicroservice.Services;

public class RabbitMQListenerService : IRabbitMQListenerService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;
    
    public RabbitMQListenerService(IOptions<RabbitMQConfiguration> options)
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
    }

    public void StartListening(string queueName, Func<BasicDeliverEventArgs, Task> messageHandler)
    {
        _channel.QueueDeclare(queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        _consumer.Received += async (model, ea) =>
        {
            await messageHandler(ea);  
            
            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(
            queue: queueName,
            autoAck: false,
            consumer: _consumer);
    }
    
    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}