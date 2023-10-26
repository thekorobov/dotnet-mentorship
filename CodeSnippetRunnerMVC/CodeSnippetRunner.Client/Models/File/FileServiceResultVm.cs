using CodeSnippetRunner.Client.Models.Code;

namespace CodeSnippetRunner.Client.Models.File;

public class FileServiceResultVm
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
    public string InputCode { get; set; }
    public CodeOutputVm OutputModel { get; set; }
}