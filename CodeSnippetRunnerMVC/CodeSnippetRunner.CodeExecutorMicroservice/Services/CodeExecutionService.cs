using CodeSnippetRunner.CodeExecutorMicroservice.Models;
using CodeSnippetRunner.CodeExecutorMicroservice.Services.Interfaces;
using CodeSnippetRunner.CodeExecutorMicroservice.Utils;
using CodeSnippetRunner.CodeExecutorMicroservice.Utils.Constants;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace CodeSnippetRunner.CodeExecutorMicroservice.Services
{
    public class CodeExecutionService : ICodeExecutionService
    {

        public async Task<CodeOutput> ExecuteCodeAsync(string inputCode)
        {
            if (CodeSecurityHelper.ContainsRestrictedPatterns(inputCode))
            {
                return new CodeOutput
                {
                    ErrorOutput = "The code contains restricted patterns."
                };
            }

            using var writer = new StringWriter();
            Console.SetOut(writer);
            var options = CreateScriptOptions();

            try
            {
                using (var cts = new CancellationTokenSource())
                {
                    cts.CancelAfter(TimeSpan.FromSeconds(10));
                    var result = await CSharpScript.EvaluateAsync(inputCode, options, cancellationToken: cts.Token);

                    return new CodeOutput
                    {
                        Result = result?.ToString(),
                        ConsoleOutput = writer.ToString(),
                        Content = inputCode
                    };
                }
            }
            catch (CompilationErrorException compilationError)
            {
                return new CodeOutput
                {
                    ErrorOutput = CodeSecurityHelper.FormatCompilationError(compilationError),
                    Content = inputCode
                };
            }
            catch (OutOfMemoryException)
            {
                return new CodeOutput
                {
                    ErrorOutput = "Error: Code execution was halted due to excessive memory usage.",
                    Content = inputCode
                };
            }
            catch (OperationCanceledException)
            {
                return new CodeOutput
                {
                    ErrorOutput = "Error: Code execution was halted due to timeout.",
                    Content = inputCode
                };
            }
            catch (Exception ex)
            {
                return new CodeOutput
                {
                    ErrorOutput = ex.Message,
                    Content = inputCode
                };
            }
            finally
            {
                Console.SetOut(Console.Out);
            }
        }

        private ScriptOptions CreateScriptOptions()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
                .ToArray();

            var defaultImports = CodeConstants.DefaultImports.ToList();

            defaultImports.RemoveAll(ns => CodeConstants.RestrictedImports.Contains(ns));

            return ScriptOptions.Default
                .AddReferences(assemblies)
                .AddImports(defaultImports);
        }
    }
}
