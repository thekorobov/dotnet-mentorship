namespace CodeSnippetRunner.CodeExecutorMicroservice.Utils.Constants
{
    public static class CodeConstants
    {

        public static readonly string[] DefaultImports = { "System", "System.Linq", "System.Collections.Generic" };
        public static readonly string[] RestrictedImports = { "System.Net", "System.Diagnostics", "System.IO", "Microsoft.Win32", "System.Data", "System.Reflection" };
    }
}
