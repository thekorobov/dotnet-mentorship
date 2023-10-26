# CodeSnippetRunnerMVC 🟡🚀

## 🔍 Introduction

Welcome to **CodeSnippetRunnerMVC**! 🎉 This innovative project is engineered for executing C# code snippets swiftly and securely. Whether you need to run a snippet or upload a file, CodeSnippetRunner has got you covered!

## 📈 Projects Overview

### 🛠️ Tech Stack

- **Project Type**: ASP.NET Core MVC
- **Architecture**: Microservices
- **Storage**: Azure Blob Storage, Azurite (for testing)
- **Message Queue**: RabbitMQ (used for communication between the microservices)
- **Frontend**: Razor
- **Code Editor**: Ace Editor
- **Code Execution**: Roslyn Compiler
- **Testing**: Unit and Integration Testing with xUnit

---

## 🤖 Detailed information about project

### Key Features 💡

- 🏛️ **Microservices Architecture**: The logic is meticulously separated into two microservices - the `CodeSnippetRunner.Client` MVC for code editing and file management, and the `CodeSnippetRunner.CodeExecutorMicroservice` Web API for code execution.

- 🐰 **RabbitMQ Communication**: Inter-service communication is facilitated through RabbitMQ, ensuring even if a service is down, your execution requests are queued for processing once back online.

- 🚀 **Instant Code Execution**: Input a string of code, hit run, and get immediate feedback. Our backend employs Roslyn mechanisms for compiling and executing C# code snippets.

- 🔐 **Robust Security Measures**: Protection against malicious patterns and harmful code. Also, an auto-termination feature kicks in after 10 seconds to halt any potentially endless loops.
- 💾 **Azure Blob Storage Integration**: Save your snippets or snippet files to Azure Blob Storage, edit, run or delete them anytime. Your code, accessible from anywhere!

- 📤 **File Upload & Execution**: Upload a file containing a code snippet directly into the editor, run it, and behold the output!

- 🎨 **Interactive Code Editor**: Utilizing **[Ace Editor](https://github.com/ajaxorg/ace)**, users can effortlessly write, edit and run code snippets in an intuitive online editor. Toggle between dark and light themes for a pleasant coding ambiance!

- 🖥️ **Result & Console Output**: Post-execution, observe the results, console outputs, and any errors right below the code editor.

- 💾 **Snippet Download**: Love a snippet? Download it to your machine with a simple click.

- 🧹 **Quick Editor Clear**: A dedicated button to clear the editor, making room for fresh code.

### Usage

- 📝 **Editing Code**: Click on the editor to start typing your code or toggle between dark and light themes for a personalized coding environment.

- 🚀 **Running Code**: Hit the `Execute Code` button to run your snippet and instantly see results, console output, or errors in the designated area below.

- 📤 **Uploading Files**: Click `Choose File` to select and load a C# snippet file into the editor for execution.

- 💾 **Saving Snippets**: Hit `Save Snippet to Azure`, name your snippet, and securely store it in Azure Blob Storage.

- 🖥️ **Downloading Snippets**: Click the Download icon to save a copy of your snippet to your machine.

- 🧹 **Clearing Editor**: Click the Clear Editor icon to refresh the editor for a new snippet.

---
