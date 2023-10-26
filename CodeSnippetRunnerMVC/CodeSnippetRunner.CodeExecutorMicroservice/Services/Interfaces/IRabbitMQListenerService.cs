using RabbitMQ.Client.Events;

namespace CodeSnippetRunner.CodeExecutorMicroservice.Services.Interfaces;

public interface IRabbitMQListenerService
{
    void StartListening(string queueName, Func<BasicDeliverEventArgs, Task> messageHandler);
}