# Monorepo for C#/.NET mentorship 🚀

## 🔍 Introduction

Hello there! 👋 This is where I develop my pet projects to improve my hard skills and get my first job.
I will learn how to write clean code following SOLID principles and design applications that are easy to maintain and scale.

### 🚦 Project Complexity Indicators

To give you a quick idea of the complexity involved in each project, I've categorized them into three levels:

- 🟢 **Easy**: Simple projects for beginners.
- 🟡 **Medium**: Projects with some complexity, a bit more challenging.
- 🔴 **Hard**: Complex projects that will take more time and knowledge to understand.

---

## 📈 Projects Overview

### [RemindersWebAPI](https://github.com/IT-Sadok/thekorobov#reminderswebapi--1) 🟡

### 🛠️ Tech Stack

- **Project Type**: ASP.NET Core WebAPI
- **Architecture**: 3-layer (DAL, BLL, API)
- **Key Architecture Pattern**: CQRS with Custom Mediator
- **Database**: PostgreSQL
- **Frontend**: Angular (For Google OAuth)
- **Background Jobs**: Hangfire
- **Email Service**: MailKit
- **Authentication**: Google OAuth2
- **Deployment**: Azure
- **CI/CD**: GitHub Actions
- **Concurrency Management**: Parallel execution of tasks for sending emails
- **Testing**: Unit Testing with xUnit

### [CodeSnippetRunnerMVC](https://github.com/IT-Sadok/thekorobov#codesnippetrunnermvc--1) 🟡

### 🛠️ Tech Stack

- **Project Type**: ASP.NET Core MVC
- **Architecture**: Microservices
- **Storage**: Azure Blob Storage, Azurite (for testing)
- **Message Queue**: RabbitMQ (used for communication between the microservices)
- **Frontend**: Razor
- **Code Editor**: Ace Editor
- **Code Execution**: Roslyn Compiler
- **Testing**: Unit and Integration Testing with xUnit

### [Reminders](https://github.com/IT-Sadok/thekorobov#reminders--1) 🟢

### 🛠️ Tech Stack

- **Project Type**: Console Application
- **Data Storage**: JSON files
- **Concurrency**: SemaphoreSlim for Thread Safety
- **Patterns**: Decorator and Command

---

## 🤖 Detailed information about projects

## RemindersWebAPI 🟡

Welcome to the **RemindersWebAPI**! 🎉 This project is a comprehensive WebAPI dedicated to managing and notifying users about their personal reminders while ensuring security and efficiency.

### Key Features 💡

🏛️ **3-layer Architecture**: The application is built upon a three-tier structure: DAL, BLL, and API, each serving a distinct purpose.

💾 **DAL**: Implements the **UnitOfWork** and **Repository** patterns for effective data handling.

🚀 **BLL**: Employs **CQRS** with a **custom mediator**. Also houses various services ensuring business logic separation.

🌍 **API**: Comprises controllers and middleware to handle and route incoming requests.

🧑 **User Management**: Create, update, and get user data. JWT tokens integrated for authentication. Role-based authorization using `IdentityRole` ensures only administrators can delete users.

📝 **Reminders**: Users have full control over their reminders: add, update, delete, or get.

🔑 **Google OAuth 2.0**: Offers both traditional and Google-based registration and authentication.

#### 📧 Notifications & Scheduling with Hangfire

- **Periodic Scans**: The system periodically checks for reminders due for the day for each verified user.
- **Email Notifications**: Identified reminders are then emailed to users, ensuring they're always up-to-date with their tasks.
- **Parallel Email Dispatch**: The emails are sent out in parallel to ensure quick completion of the operation.
- **Efficient Database Queries**: Makes use of `IQueryable` for efficient and swift database interactions.

#### 📊 Middleware & Monitoring

- **Exception Handling**: The `GlobalExceptionHandlerMiddleware` ensures uniformity in exception management.
- **Unified Responses**: With `ResponseWrapperMiddleware`, every server response adheres to a consistent format.
- **System Health Monitoring**: `HealthCheck` stays vigilant, periodically assessing the database connection and logging its status, ensuring everything is up and running.

---

## CodeSnippetRunnerMVC 🟡

Welcome to **CodeSnippetRunnerMVC**! 🎉 This innovative project is engineered for executing C# code snippets swiftly and securely. Whether you need to run a snippet or upload a file, CodeSnippetRunner has got you covered!

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

## Reminders 🟢

Welcome to the Reminders Console Application! 🎉 This project serves as a reminder management tool that offers features for adding, deleting, and filtering reminders. Whether you need to view all reminders, get reminders for certain dates, or apply filters to dates before or after a certain moment, this app will help you. 🗓️

### Key Features 💡

- 📝 **Reminder Management**: Easily add, delete, and filter reminders through user-friendly commands, providing a streamlined experience for managing your tasks.
- 💾 **Persistence**: All your important reminder data is stored efficiently using JSON files, ensuring your information is saved and available even after application restarts.
- 🛡️ **Concurrency Safeguard**: The application incorporates a SemaphoreSlim mechanism to ensure thread safety, making it reliable and capable of handling multiple requests simultaneously without data corruption.
- ⏩ **Asynchronous Execution**: Every command within the application is designed to work asynchronously, promoting responsiveness and efficient resource utilization.
- 🔄 **Decorator Pattern**: The implementation of the Decorator pattern enhances file management, offering both a basic file service and a thread-safe version, thus maintaining code modularity and reusability.
- 🎮 **Command Pattern**: To manage the multitude of commands and promote clean code architecture, the Command pattern is employed, enhancing maintainability and reducing coupling.
- 📚 **Naming Convention and SOLID Principles**: The project adheres to consistent naming conventions and follows SOLID principles to ensure a well-structured, easily maintainable codebase.
- ⚙️ **Minimal Dependencies**: The application's logic is organized in a way that minimizes unnecessary dependencies, making it lightweight and efficient.

### Usage

The Reminders Console Application offers a variety of commands to interact with your reminders. Here are some examples:

- `add` - Add a new reminder.
- `delete` - Delete an existing reminder.
- `get-all` - Get all reminders.
- `get-by-date` - Get reminders based on a specific date.
- `filter-before` - Filter reminders before a given date.
- `filter-after` - Filter reminders after a given date.
- `search` - Search for reminders based on a keyword.

---
