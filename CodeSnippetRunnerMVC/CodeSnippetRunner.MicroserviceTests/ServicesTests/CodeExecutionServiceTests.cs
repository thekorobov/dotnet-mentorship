using CodeSnippetRunner.CodeExecutorMicroservice.Services;

namespace CodeSnippetRunner.MicroserviceTests.ServicesTests
{
    public class CodeExecutionServiceTests
    {
        [Theory]
        [InlineData("Console.WriteLine(\"wooof\");", "wooof")]
        [InlineData("Console.WriteLine(2+2);", "4")]
        [InlineData("Console.WriteLine(Math.PI);", "3.14")]
        public async Task ExecuteCodeAsync_ValidInput_SuccessfulResult(string inputCode, string expectedCodeResult)
        {
            //Arrange
            var codeExecutor = new CodeExecutionService();        
            
            //Act
            var result = await codeExecutor.ExecuteCodeAsync(inputCode);
            
            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrWhiteSpace(result.ErrorOutput));
            Assert.Contains(expectedCodeResult, result.ConsoleOutput);
        }

        [Fact]
        public async Task ExecuteCodeAsync_InvalidInput_ReturnsCompilationErrorCodeOutput()
        {
            //Arrange
            var codeExecutor = new CodeExecutionService();
            var inputCode = "using System;\r\nusing System.Linq;\r\nusing System.Collections.Generic;\r\nConsole.WriteLine(\"woof\"";
            
            //Act
            var result = await codeExecutor.ExecuteCodeAsync(inputCode);
            
            //Assert
            Assert.NotNull(result);
            Assert.Null(result.ConsoleOutput);
            Assert.NotEmpty(result.ErrorOutput);
        }
        
        [Theory]
        [InlineData("while(true){ }")]
        [InlineData("for(;;){ }")]
        [InlineData("GC.Collect()")]
        public async Task ExecuteCodeAsync_InvalidInput_ReturnsRestrictedPatternsCodeOutput(string codePattern)
        {
            //Arrange
            var codeExecutor = new CodeExecutionService();
            var inputCode = $"using System;\r\nusing System.Linq;\r\nusing System.Collections.Generic;\r\n {codePattern}";
            
            //Act
            var result = await codeExecutor.ExecuteCodeAsync(inputCode);
            
            //Assert
            Assert.NotNull(result);
            Assert.Null(result.ConsoleOutput);
            Assert.NotEmpty(result.ErrorOutput);
            Assert.Contains("restricted patterns", result.ErrorOutput);
        }
    }
}
