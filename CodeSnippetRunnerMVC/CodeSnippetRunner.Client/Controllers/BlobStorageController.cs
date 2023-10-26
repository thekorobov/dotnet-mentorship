using CodeSnippetRunner.Client.Models.File;
using CodeSnippetRunner.Client.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodeSnippetRunner.Client.Controllers;

[ApiController]
[Route("api/snippets")]
public class BlobStorageController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;

    public BlobStorageController(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetListOfBlobsAsync()
    {
        var blobNames = await _blobStorageService.GetListOfBlobNamesAsync();
        return Ok(blobNames);
    }

    [HttpDelete("{blobFileName}")]
    public async Task<IActionResult> DeleteBlobAsync(string blobFileName)
    {
        await _blobStorageService.DeleteBlobAsync(blobFileName);
        return Ok();
    }
    
    [HttpPut("{blobFileName}")]
    public async Task<IActionResult> UpdateBlobContent(string blobFileName, [FromBody] ContentInputVm contentInput)
    {
        await _blobStorageService.UploadBlobAsync(blobFileName, contentInput.Content);
        return Ok();
    }
}