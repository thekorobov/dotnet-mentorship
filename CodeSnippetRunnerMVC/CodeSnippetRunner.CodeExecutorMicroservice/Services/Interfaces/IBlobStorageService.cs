namespace CodeSnippetRunner.CodeExecutorMicroservice.Services.Interfaces;

public interface IBlobStorageService
{
    Task<string> ReadCodeAsync(string blobFileName, string sasToken);
}