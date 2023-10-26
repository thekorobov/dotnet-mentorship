using System.Text;
using CodeSnippetRunner.Client.Models.File;
using CodeSnippetRunner.Client.Services.Interfaces;

namespace CodeSnippetRunner.Client.Services;

public class FileService : IFileService
{
    public bool IsValidFile(IFormFile file)
        => file != null && (file.Length != 0 && file.Length < 10_240);
    
    public async Task<FileServiceResultVm> ReadFileContents(IFormFile file)
    {
        try
        {
            using var streamReader = new StreamReader(file.OpenReadStream());
            var content = await streamReader.ReadToEndAsync();
            return new FileServiceResultVm { IsSuccess = true, InputCode = content };
        }
        catch (Exception ex)
        {
            return new FileServiceResultVm { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<FileServiceResultVm> ValidateFile(IFormFile file)
    {
        return IsValidFile(file) ?
            await ReadFileContents(file) :
            new()
             {
                 IsSuccess = false,
                 ErrorMessage = "Wrong file"
             };
    }

    public FileDownloadResultVm WriteCodeToFile(string content)
    {
        var contentBytes = Encoding.UTF8.GetBytes(content);
        var contentType = "text/plain";

        return new FileDownloadResultVm
        {
            ContentBytes = contentBytes,
            ContentType = contentType,
            FileName = "code.txt"
        };
    }
}