namespace CodeSnippetRunner.Client.Models.File;

public class FileDownloadResultVm
{
    public byte[] ContentBytes { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; }
}