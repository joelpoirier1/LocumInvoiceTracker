# Locum Invoice Tracker

A small full-stack Blazor application for recording locum veterinary shifts and generating downloadable Excel reports.

The project demonstrates practical use of C#, Blazor, Entity Framework Core, SQLite, and ClosedXML in a complete workflow.

## Screenshots

### Shift Management

![Shift management page](docs/screenshots/SS2.png)

### Excel Reports

![Excel report page](docs/screenshots/SS1.png)

![Generated report](docs/screenshots/SS3.png)

## Features

* Add and view completed locum shifts
* Associate shifts with veterinary hospitals
* Store shift information in a SQLite database
* Calculate shift totals using hours worked and hourly rate
* Filter reports by hospital and date range
* Generate downloadable Excel reports
* Validate user input through Blazor forms
* Persist data using Entity Framework Core

## Technology Stack

* **C# / .NET 10**
* **Blazor Web App**
* **Entity Framework Core**
* **SQLite**
* **ClosedXML**
* **Bootstrap**

## Application Flow

The application connects the technologies through the following workflow:

1. A user enters shift information through a Blazor form.
2. Blazor validates the form and submits the data.
3. Entity Framework Core saves the shift to a SQLite database.
4. Saved shifts are queried and displayed in the Blazor interface.
5. The reporting endpoint retrieves shifts using Entity Framework Core.
6. ClosedXML generates an Excel workbook from the retrieved data.
7. The completed workbook is returned as a downloadable `.xlsx` file.

## Project Structure

```text
LocumInvoiceTracker
├── Components
│   ├── Layout
│   └── Pages
│       ├── Shifts.razor
│       └── Reports.razor
├── Data
│   └── AppDbContext.cs
├── Models
│   ├── Hospital.cs
│   └── WorkShift.cs
├── Services
│   └── ExcelExportService.cs
├── Migrations
├── Program.cs
└── appsettings.json
```

## Entity Framework Core

Entity Framework Core is used to:

* Define the hospital and shift entities
* Configure the relationship between hospitals and shifts
* Create and update the database through migrations
* Perform asynchronous CRUD operations
* Query related data using LINQ
* Seed initial hospital records

Each database operation creates a dedicated `AppDbContext` through `IDbContextFactory<AppDbContext>`.

## Blazor

Blazor provides the user interface and application interaction, including:

* Reusable Razor components
* Interactive server rendering
* Page routing
* Form validation
* Data binding
* Event handling
* Asynchronous database operations

## ClosedXML

ClosedXML generates Excel shift reports containing:

* Shift dates
* Hospital names
* Hours worked
* Hourly rates
* Individual shift totals
* Overall report totals
* Basic spreadsheet formatting

## Getting Started

### Prerequisites

Install:

* .NET 10 SDK
* Git
* Visual Studio Code, Visual Studio, or another compatible editor

Confirm that the .NET SDK is available:

```powershell
dotnet --version
```

### Clone the Repository

```powershell
git clone YOUR_REPOSITORY_URL
cd LocumInvoiceTracker
```

Replace `YOUR_REPOSITORY_URL` with the URL of this repository.

### Restore Dependencies

```powershell
dotnet restore
```

### Create the Database

Apply the included Entity Framework Core migrations:

```powershell
dotnet ef database update
```

If the Entity Framework command-line tool is not installed:

```powershell
dotnet tool install --global dotnet-ef
```

### Run the Application

```powershell
dotnet run
```

Open the local address displayed in the terminal.

The main application pages are:

```text
/shifts
/reports
```

## Database

The application uses SQLite for local development.

The database is generated locally and is not committed to the repository. Running the Entity Framework migration command creates it automatically.

## Future Improvements

Potential additions include:

* Editing existing shifts
* Marking shifts as paid
* Creating and managing hospitals
* Invoice numbers and tax calculations
* PDF invoice generation
* Authentication and user accounts
* Automated tests
* Cloud deployment

## Purpose

This project was created as a focused demonstration of building a connected application using Blazor, Entity Framework Core, ClosedXML, and C#.

It covers the complete path from user input and database persistence to report generation and file download.
