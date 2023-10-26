namespace CodeSnippetRunner.Client.Models.Messaging;

public class RabbitMQConfiguration
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string VirtualHost { get; set; }
    public string Hostname { get; set; }
    public int Port { get; set; }
    public string RequestQueueName { get; set; }
    public string ResponseQueueName { get; set; }
}