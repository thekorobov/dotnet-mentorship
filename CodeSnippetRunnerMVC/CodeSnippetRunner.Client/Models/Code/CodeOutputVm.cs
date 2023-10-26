namespace CodeSnippetRunner.Client.Models.Code;

public class CodeOutputVm
{
    public string Content { get; set; }
    public string Result { get; set; }
    public string ConsoleOutput { get; set; }
    public string ErrorOutput { get; set; }
}