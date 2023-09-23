# Finance Management Application

## Description

This application provides finance management features to help users better manage their personal finances. By utilizing an easy-to-use interface and secure registration and authentication system, users can manage their budgets, track their expenses, and gain insights into their financial habits.
## Features

- User registration and authentication
- Budget creation and management
- Expense tracking and categorization
- Financial reporting and analysis

## Getting Started

Follow these instructions to set up the project locally.

### Prerequisites

- .NET 5.0 or later
- Visual Studio 2019 or Visual Studio Code with C# extension
- Microsoft SQL Server (or another compatible database)

### Setup

1. Clone the repository.
2. Navigate to the solution folder.
3. Restore the .NET and NuGet packages: dotnet restore

4. Update the connection string in the `appsettings.json` file located in the `BookStore.API` project folder:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
  },
  // ...
}
Replace YOUR_SERVER, YOUR_DATABASE, YOUR_USER, and YOUR_PASSWORD with your database information.

5. Run the database migrations to create and update the necessary database schema:
        dotnet ef database update --project Inventory.DAL

## Running the Application

1. Ensure that the `BookStore.API` project is set as the startup project in Visual Studio or in your `launchSettings.json` file.

2. Start the application:

   - In Visual Studio: Press `F5` or click the "Start" button with the green triangle.
   - In Visual Studio Code: Press `F5`, or open a terminal and run:
     
     ```
     dotnet run --project BookStore.API
     ```
     
3. Once the application is running, open a web browser and navigate to `https://localhost:5001` (or the address displayed in the terminal).

4. You can now start using the Inventory System to manage your products.


## Feedback
This task was hard for implementing with ChatGPT. 
It didn't understand third-party API implementation, often forgot previous code. 
That's why I didn't finish this task completely, because it can take much more time. 
I did the task for approximately 8 hours. Many parts of code were not ready; 
I changed Repositories, UnitOfWork, Services, and UserAuthServices. 
Chat often forgot its previous recommendations and code. 
ChatGPT didn't understand how to format README files, UnitTests.
Also business logic was complicated for him too.
Useful prompts were: architecture, clean code, and unit test writing.