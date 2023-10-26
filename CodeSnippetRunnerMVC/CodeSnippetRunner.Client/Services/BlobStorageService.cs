using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using CodeSnippetRunner.Client.Services.Interfaces;

namespace CodeSnippetRunner.Client.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _containerClient;
    private readonly string _containerName = "snippets";

    public BlobStorageService(string connectionString)
    {
        _blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
    }
    
    public async Task<BlobClient> UploadBlobAsync(string blobName, string code)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(blobName);
        await using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(code));
        await blobClient.UploadAsync(contentStream, overwrite: true);

        return blobClient;
    }

    public async Task<Stream> DownloadBlobAsync(string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
    
        if (await blobClient.ExistsAsync())
        {
            return await blobClient.OpenReadAsync();
        }
        return null;
    }

    public async Task DeleteBlobAsync(string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<IEnumerable<string>> GetListOfBlobNamesAsync()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        var blobNames = new List<string>();
        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            blobNames.Add(blobItem.Name);
        }
        return blobNames.OrderBy(name => name);
    }
    
    public Uri GenerateSasToken(string blobName)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);

        if (!blobClient.CanGenerateSasUri) return null;

        var expiredDate = DateTimeOffset.UtcNow.AddHours(1);
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
            BlobName = blobClient.Name,
            Resource = "b",
            ExpiresOn = expiredDate,
            CacheControl = "max-age" + expiredDate
        };
        sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);

        var sasUri = blobClient.GenerateSasUri(sasBuilder);
        return sasUri;
    }
}