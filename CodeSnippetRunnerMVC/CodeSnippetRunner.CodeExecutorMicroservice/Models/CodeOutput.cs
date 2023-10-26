namespace CodeSnippetRunner.CodeExecutorMicroservice.Models
{
    public class CodeOutput
    {
        public string Content { get; set; }
        public string Result { get; set; }
        public string ConsoleOutput { get; set; }
        public string ErrorOutput { get; set; }
    }
}
