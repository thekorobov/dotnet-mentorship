using Azure.Storage.Blobs;

namespace CodeSnippetRunner.Client.Services.Interfaces;

public interface IBlobStorageService
{
    Task<BlobClient> UploadBlobAsync(string blobName, string code);
    Task<Stream> DownloadBlobAsync(string blobName);
    Task DeleteBlobAsync(string blobName);
    Task<IEnumerable<string>> GetListOfBlobNamesAsync();
    Uri GenerateSasToken(string blobName);
}