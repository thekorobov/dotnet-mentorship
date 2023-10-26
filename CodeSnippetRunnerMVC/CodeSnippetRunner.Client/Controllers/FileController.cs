using CodeSnippetRunner.Client.Models.File;
using CodeSnippetRunner.Client.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodeSnippetRunner.Client.Controllers;

[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("download-code")]
    public async Task<IActionResult> DownloadCode([FromBody] ContentInputVm contentInput)
    {
        var fileResult = _fileService.WriteCodeToFile(contentInput.Content);
        return File(fileResult.ContentBytes, fileResult.ContentType, fileResult.FileName);
    }
}