using CodeSnippetRunner.Client.Models.Code;
using CodeSnippetRunner.Client.Models.File;
using CodeSnippetRunner.Client.Models.Messaging;
using CodeSnippetRunner.Client.Services.Interfaces;
using CodeSnippetRunner.Client.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CodeSnippetRunner.Client.Controllers;

[ApiController]
[Route("api/executor")]
public class CodeExecutionController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IRabbitMQService _rabbitMqService;

    public CodeExecutionController(
        IFileService fileService, 
        IBlobStorageService blobStorageService, 
        IRabbitMQService rabbitMqService)
    {
        _fileService = fileService;
        _blobStorageService = blobStorageService;
        _rabbitMqService = rabbitMqService;
    }

    [HttpPost("snippet")]
    public async Task<IActionResult> ExecuteSnippetAsync([FromBody] CodeInputVm codeInput)
    {
        if (string.IsNullOrEmpty(codeInput.InputCode))
        {
            return BadRequest("Code content is required.");
        }

        var blobFileName = FileNameHelper.ModifyFileName();
        await _blobStorageService.UploadBlobAsync(blobFileName, codeInput.InputCode);

        return await ExecuteCodeAsync(blobFileName, true);
    }

    [HttpPost("file")]
    public async Task<IActionResult> ExecuteFileAsync([FromForm] CodeFileInputVm codeFileInput)
    {
        var executionResult = await _fileService.ValidateFile(codeFileInput.CodeFile);
        if (!executionResult.IsSuccess)
            return BadRequest(executionResult.OutputModel);
        
        var blobFileName = FileNameHelper.ModifyFileName(codeFileInput.CodeFile.FileName);
        await _blobStorageService.UploadBlobAsync(blobFileName, executionResult.InputCode);

        return await ExecuteCodeAsync(blobFileName, true);
    }

    [HttpPost("storage-file")]
    public async Task<IActionResult> ExecuteBlobFileAsync([FromBody] BlobFileExecutionVm blobFileExecution)
    {
        return await ExecuteCodeAsync(blobFileExecution.BlobFileName, false);
    }

    [HttpPost("save-snippet")]
    public async Task<IActionResult> SaveSnippetToBlobAsync([FromBody] FileDownloadInputVm fileDownloadInput)
    {
        if (string.IsNullOrEmpty(fileDownloadInput.InputCode))
        {
            return BadRequest("Code content is required.");
        }

        await _blobStorageService.UploadBlobAsync(FileNameHelper.ModifyFileName(fileDownloadInput.FileName), fileDownloadInput.InputCode);
        return Ok();
    }
    
    private async Task<IActionResult> ExecuteCodeAsync(string blobFileName, bool deleteBlob)
    {
        var sasToken = _blobStorageService.GenerateSasToken(blobFileName);

        var httpClientDataModel = new HttpClientDataVm
        {
            BlobFileName = blobFileName,
            SasToken = sasToken.ToString()
        };

        var response = await _rabbitMqService.SendAndReceive(httpClientDataModel);
    
        if(deleteBlob)
        {
            await _blobStorageService.DeleteBlobAsync(blobFileName);
        }
    
        return string.IsNullOrEmpty(response.ErrorOutput) ? Ok(response) : BadRequest(response);
    }
}
