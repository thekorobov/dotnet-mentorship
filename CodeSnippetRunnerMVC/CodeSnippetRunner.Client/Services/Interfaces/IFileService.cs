using CodeSnippetRunner.Client.Models.File;

namespace CodeSnippetRunner.Client.Services.Interfaces;

public interface IFileService
{
    bool IsValidFile(IFormFile file);
    Task<FileServiceResultVm> ReadFileContents(IFormFile file);
    Task<FileServiceResultVm> ValidateFile(IFormFile file);
    FileDownloadResultVm WriteCodeToFile(string content);
}