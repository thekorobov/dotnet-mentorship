# RemindersWebAPI 🟡🚀

## 🔍 Introduction

Welcome to the **RemindersWebAPI**! 🎉 This project is a comprehensive WebAPI dedicated to managing and notifying users about their personal reminders while ensuring security and efficiency.

## 📈 Projects Overview

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

---

## 🤖 Detailed information about project

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
