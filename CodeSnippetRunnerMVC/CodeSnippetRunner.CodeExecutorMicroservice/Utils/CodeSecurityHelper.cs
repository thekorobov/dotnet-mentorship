using Microsoft.CodeAnalysis.Scripting;
using System.Text.RegularExpressions;

namespace CodeSnippetRunner.CodeExecutorMicroservice.Utils
{
    public static class CodeSecurityHelper
    {
        public static string FormatCompilationError(CompilationErrorException compilationError)
        {
            var missingErrors = compilationError.Diagnostics
                .Where(d => d.Id == "CS0246")
                .Select(d =>
                {
                    var match = Regex.Match(d.GetMessage(), @"'(.+?)'");
                    return match.Success
                        ? $"Error: The assembly or namespace \"{match.Groups[1].Value}\" could not be found. Please ensure you have added all necessary assemblies or namespaces."
                        : d.GetMessage();
                });

            return missingErrors.Any()
                ? string.Join("\n", missingErrors)
                : string.Join("\n", compilationError.Diagnostics);
        }

        public static bool ContainsRestrictedPatterns(string code)
        {
            var restrictedPatterns = new List<string>
        {
            "Thread",               // Restricting the use of System.Threading.Thread
            "Task.Run",             // Restricting the asynchronous execution of tasks
            "while(true)",          // Infinite loops
            "for(;;)",              // Another variant of an infinite loop
            "Process.Start",        // Prohibiting the start of processes
            "Assembly.Load",        // Prohibiting the loading of assemblies
            "AppDomain",            // Operations with AppDomain can be dangerous
            "File.",                // File operations (if you need to work with files, consider restricting only specific methods)
            "Mutex",                // Restriction on using Mutex, which might lock resources
            "ThreadPool",           // Prohibiting access to the thread pool
            ".GetHashCode",         // GetHashCode method can be used for DOS attacks
            "GC.Collect",           // Prohibiting forced garbage collection
            "Activator.CreateInstance", // Prohibiting the dynamic creation of objects
            "Marshal",              // Operations with Marshal can be dangerous
            "DllImport",            // Prohibiting library imports
            "Reflection",           // Prohibiting reflection
            "Unsafe",               // Restricting the use of unsafe code
            "fixed",                // Restricting the use of the 'fixed' keyword
            "stackalloc",           // Restricting memory allocation in the stack
            "MemoryMappedFile",     // Restrictions on working with MemoryMappedFile
            "Timer",
            "System.Diagnostics",   
            "Environment.Exit",    
            "Environment.FailFast",
        };

            return restrictedPatterns.Any(pattern => code.Contains(pattern));

        }
    }
}
