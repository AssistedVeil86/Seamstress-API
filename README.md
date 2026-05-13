# Seamstress Management API

> A Web API backend for managing orders, expenses, and financial records for a seamstress business, built with .NET 10 and Vertical Slice Architecture

## 📋 Overview

This is a Web API backend project built with C# 14 and .NET 10 using Minimal APIs, following **Vertical Slice Architecture**, designed to help a seamstress business owner digitalize and manage their orders, expenses, and overall financial situation.

Developed in just **3 weeks**, this API is intended for direct use by the business owner, providing a clear and structured way to track income, deductions, and weekly expenses — replacing manual and informal record-keeping with a reliable digital solution.

## 🚀 Technologies Used

- **.NET 10** — Latest .NET framework
- **C# 14** — Modern C# features
- **ASP.NET Core** — High-performance web framework
- **ASP.NET Identity** — Authentication and user management
- **PostgreSQL** — Robust relational database
- **QuestPDF** — PDF generation for financial reports
- **Hangfire** — Background job scheduling for automated reports
- **FluentValidation** — Clean and expressive input validation
- **OpenAPI / Scalar** — API documentation
- **Docker** — Containerization for easy deployment
- **SMTP** — Email delivery for notifications and reports

## ✨ Features

The core problem this API solves is that the business owner had no structured way to track the financial situation of their seamstress business — including expenses, deductions, and income. The solution digitalizes the entire workflow: from order registration to automated weekly financial reporting.

Here's a full breakdown of its capabilities:

### 🧵 Order Registration
Allows the owner to maintain a digital registry of all orders, both currently in progress and completed during the week, along with all their relevant details and information.

### 💸 Expense Registration
Follows a **weekly schedule**, allowing the owner to log expenses by type, including a description and the amount spent. This keeps a clean and organized record of all outgoing costs throughout the week.

### 📊 Automatic Weekly Financial Reports
Using **Hangfire Background Jobs**, financial reports are generated automatically at the end of each week, giving the owner a clear picture of:
- Gross income for the week
- Deductions applied
- Net profit at the end of the week

### 📄 PDF Report Generation
Financial reports can be exported as **PDF files** using **QuestPDF**, allowing the owner to save and archive their weekly financial records over time.

### 🔐 Authentication System
The API is protected with **ASP.NET Identity**, ensuring only authorized users can access and interact with the system, preventing unwanted access or misuse.

## 🏗️ Building Process

The development process was quick once the owner's problem was properly understood and the workflow they were already used to was taken into account. The main challenge was not the code itself, but the **definition phase**: understanding the problem, outlining the right features, designing the database schema, and only then putting everything together in code.

This project was a great real-world exercise in working directly with an entrepreneur to analyze a business problem, propose a solution, and implement it in a way that truly adjusts to their needs and daily workflow.

## 🔧 Areas for Improvement

- **Unit Testing**: Due to time constraints, unit tests were not implemented — despite being absolutely essential to any production-ready project. Adding comprehensive test coverage remains a pending priority.
- **Data Seeding**: Data seeding was handled with `UserManager`, but it could be improved by using the `UseSeeding` method introduced in .NET 9 for a cleaner and more integrated approach.

## 🚀 How to Run the Project

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) installed on your machine
- PostgreSQL database server
- An IDE of your choice:
  - Visual Studio
  - JetBrains Rider
  - Visual Studio Code

### Setup Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/seamstress-api.git
   cd seamstress-api
   ```

2. **Open the project**
   - Open the `.slnx` file with your preferred IDE

3. **Configure the database**
   - Update the connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=seamstress;Username=your_user;Password=your_password"
     }
   }
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the project**
   ```bash
   dotnet run
   ```

6. **Access the API documentation**
   - Navigate to the Scalar UI to explore all available endpoints

And that's it — the project is now up and running! 🎉