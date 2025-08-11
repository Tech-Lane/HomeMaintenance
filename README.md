# Home Maintenance Application

A comprehensive cross-platform application for managing all aspects of home maintenance, from tracking routine maintenance and chores to planning major projects and storing important appliance information.

## Project Overview

This application is designed to be cross-platform, accessible via web browsers and native mobile applications, with a flexible architecture that allows for future expansion through custom plug-ins.

## Architecture

### Core Architectural Decisions

- **Hybrid MVVM and Service-Oriented Architecture**
  - .NET MAUI (Mobile): Strictly follows the MVVM pattern
  - Blazor (Web): Component-based architecture with service-oriented approach
  - Backend: RESTful API for data synchronization and centralized business logic

### Project Structure

The solution is organized into the following projects:

#### Core Projects
- **HomeMaintenance.Core** (.NET Standard 2.1): Contains all shared code including data models, business logic services, interfaces, and view models
- **HomeMaintenance.Data** (.NET 8.0): Handles all data persistence using Entity Framework Core
- **HomeMaintenance.Plugin.Abstractions** (.NET Standard 2.1): Defines interfaces and contracts for plugins

#### Application Projects
- **HomeMaintenance.API** (ASP.NET Core Web API): Backend REST API for data synchronization
- **HomeMaintenance.Web** (Blazor Web App): Web application with component-based architecture
- **HomeMaintenance.MAUI** (.NET MAUI App): Cross-platform mobile application

#### Test Projects
- **HomeMaintenance.Tests.Unit** (xUnit): Unit tests for Core and Data projects
- **HomeMaintenance.Tests.Integration** (xUnit): Integration tests for the API

## Data Models

The application includes a rich set of data models:

### Core Entities
- **Appliance**: Stores details about each appliance, including make, model, purchase date, and warranty information
- **ApplianceManual**: Links to stored manual files (PDF, etc.)
- **MaintenanceTask**: Represents recurring maintenance tasks
- **TaskCompletion**: Tracks when maintenance tasks are completed
- **Chore**: One-time or recurring household chores
- **ChoreCompletion**: Tracks when chores are completed
- **Project**: Home improvement projects with budget and timeline
- **ProjectTask**: Tasks within a project
- **CostEstimate**: Stores cost estimates for projects or repairs
- **FloorPlan**: Contains data for rendering floor plans

### Base Entity
All entities inherit from `BaseEntity` which provides:
- Unique identifier (Id)
- Creation and modification timestamps
- Soft delete functionality (IsActive flag)

## Data Persistence

### Local (MAUI)
- **SQLite**: For structured, relational data
- **File System**: For storing appliance manuals and other large files
- **SecureStorage**: For sensitive data like API keys
- **Preferences**: For non-sensitive user settings

### Central (Web and API)
- **SQL Server/PostgreSQL**: Robust relational database
- **Entity Framework Core**: ORM for database interactions

### Data Synchronization
The MAUI app will periodically sync its local SQLite database with the central database via the REST API, enabling offline access.

## Plugin Architecture

The application implements a plugin architecture for extensibility:

### Plugin Contract
- Common interface `IHomeMaintenancePlugin` defines required methods
- Plugins must implement Initialize, Execute, and Shutdown methods
- Plugin metadata includes Name, Description, Version, Author, etc.

### Plugin Discovery and Loading
- Main application scans designated "plugins" directory
- Uses reflection to load assemblies and instantiate plugin types
- Plugins can contribute to UI by providing Blazor components or MAUI views

## Current Implementation Status

### ✅ Completed (Phase 1 - Foundation)
- [x] Solution structure and all projects created
- [x] Core data models implemented with full XML documentation
- [x] Entity Framework Core DbContext with Fluent API configuration
- [x] Repository pattern implementation with generic repository
- [x] Unit of Work pattern for transaction management
- [x] Service interfaces defined for business logic
- [x] Plugin architecture abstractions
- [x] Expression extensions for repository filtering
- [x] Business service implementations (ApplianceService, MaintenanceService, ChoreService)
- [x] API controllers and endpoints (Appliances, MaintenanceTasks, Chores)
- [x] Database migrations and database creation
- [x] API testing and validation

### ✅ Completed (Phase 2 - Web Application Foundation) - **PRODUCTION READY** 🚀
- [x] Blazor web application development
- [x] Main layout and navigation with proper routing
- [x] Dashboard page with real-time statistics and alerts
- [x] Responsive design with Bootstrap styling
- [x] Navigation menu with proper icons and structure
- [x] Database seeding with sample data
- [x] **Full CRUD operations for Appliances** (List, Add, View Details, Edit, Delete)
- [x] **Full CRUD operations for Maintenance Tasks** (List, Add, Edit, Delete, Complete)
- [x] **Full CRUD operations for Chores** (List, Add, View Details, Edit, Delete, Complete)
- [x] Data integration with API
- [x] Form components for adding/editing all entities
- [x] Detailed views with comprehensive information
- [x] Advanced filtering and pagination for all features
- [x] Statistics and overview cards for all features
- [x] Real-time dashboard with overdue alerts and due items
- [x] Completion history tracking
- [x] **Confirmation dialogs for safe deletion**
- [x] **Task/chore completion modals with detailed tracking**
- [x] Professional UI with loading states and error handling
- [x] **Notification system for user feedback**
- [x] **Reusable UI components** (modals, spinners, toasts)

### 🔄 In Progress
- [ ] Unit tests for core logic
- [ ] Integration tests for API endpoints

### 📋 Next Steps (Phase 3 - Advanced Features)
- [ ] Email notifications and reminders
- [ ] Advanced reporting and analytics
- [ ] Mobile app development (.NET MAUI)
- [ ] Multi-user support and permissions
- [ ] Advanced features (cost analytics, warranty tracking)
- [ ] State management implementation
- [ ] UI tests with bUnit

### 📋 Future Phases
- **Phase 3**: .NET MAUI Mobile Application
- **Phase 4**: Advanced Features and Polish

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- SQL Server or PostgreSQL (for production)

### Building the Solution
```bash
dotnet restore
dotnet build
```

### Running Tests
```bash
dotnet test
```

## Development Guidelines

### Code Style
- All public classes, methods, and properties must have XML documentation comments
- Follow C# coding conventions
- Use async/await patterns for all I/O operations

### Testing Strategy
- **Unit Testing**: xUnit framework with Moq for mocking
- **UI Testing**: bUnit for Blazor components, Appium for MAUI
- **Integration Testing**: API and database interaction tests

### Documentation
- **In-Code**: XML documentation comments
- **API Documentation**: Swagger/OpenAPI with Swashbuckle
- **Generated Documentation**: DocFX for comprehensive API documentation

## Contributing

1. Follow the established architecture patterns
2. Write unit tests for new functionality
3. Update documentation for any API changes
4. Ensure all builds pass before submitting

## License

[License information to be added]

## Contact

[Contact information to be added] 