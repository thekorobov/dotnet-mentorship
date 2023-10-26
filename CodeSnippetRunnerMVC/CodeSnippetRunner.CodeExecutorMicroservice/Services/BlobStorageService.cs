using System.Text;
using CodeSnippetRunner.CodeExecutorMicroservice.Services.Interfaces;
using Azure.Storage.Blobs;

namespace CodeSnippetRunner.CodeExecutorMicroservice.Services;

public class BlobStorageService : IBlobStorageService
{
    public async Task<string> ReadCodeAsync(string blobFileName, string sasToken)
    {
        if (string.IsNullOrEmpty(blobFileName) || string.IsNullOrEmpty(sasToken))
        {
            throw new ArgumentException("BlobFileName and SasToken are required.");
        }

        if (!Uri.TryCreate(sasToken, UriKind.Absolute, out var sasUri))
        {
            throw new ArgumentException("Invalid SasToken.");
        }

        var blobClient = new BlobClient(sasUri);

        using var blobStream = new MemoryStream();
        var response = await blobClient.OpenReadAsync();
        await response.CopyToAsync(blobStream);
        return Encoding.UTF8.GetString(blobStream.ToArray());
    }
}