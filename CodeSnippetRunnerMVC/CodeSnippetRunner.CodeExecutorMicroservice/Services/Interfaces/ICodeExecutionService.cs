using CodeSnippetRunner.CodeExecutorMicroservice.Models;

namespace CodeSnippetRunner.CodeExecutorMicroservice.Services.Interfaces
{
    public interface ICodeExecutionService
    {
        Task<CodeOutput> ExecuteCodeAsync(string inputCode);
    }
}
