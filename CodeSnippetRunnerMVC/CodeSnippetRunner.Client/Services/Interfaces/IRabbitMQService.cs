using CodeSnippetRunner.Client.Models.Code;

namespace CodeSnippetRunner.Client.Services.Interfaces;

public interface IRabbitMQService
{
    Task<CodeOutputVm> SendAndReceive(object message);
}