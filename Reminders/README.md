# Reminders.Console 🟢🚀

## 🔍 Introduction

Welcome to the Reminders Console Application! 🎉 This project serves as a reminder management tool that offers features for adding, deleting, and filtering reminders. Whether you need to view all reminders, get reminders for certain dates, or apply filters to dates before or after a certain moment, this app will help you. 🗓️

## 📈 Projects Overview

### 🛠️ Tech Stack

- **Project Type**: Console Application
- **Data Storage**: JSON files
- **Concurrency**: SemaphoreSlim for Thread Safety
- **Patterns**: Decorator and Command

---

## 🤖 Detailed information about project

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
