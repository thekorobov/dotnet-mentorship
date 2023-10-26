using CodeSnippetRunner.CodeExecutorMicroservice.Models;

namespace CodeSnippetRunner.CodeExecutorMicroservice.Services.Interfaces;

public interface IRabbitMQResponseService
{
    void SendResponse(string replyTo, string correlationId, CodeOutput codeOutput);
}