# MyProject - Clean Architecture with CQRS and Service Layer

This project implements Clean Architecture with CQRS pattern, MediatR, FluentValidation, AutoMapper, DuetGlobalExceptionMiddleware, and **Service Layer** for business logic separation.

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────────┐
│                        MyProject.API                             │
│  ┌────────────┐     ┌──────────────────────────────────┐       │
│  │Controllers │────▶│DuetGlobalExceptionMiddleware     │       │
│  └────────────┘     └──────────────────────────────────┘       │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                   MyProject.Application                          │
│  ┌────────────┐    ┌─────────┐    ┌──────────────────┐         │
│  │  Commands  │───▶│ MediatR │───▶│    Handlers      │         │
│  │  Queries   │    └─────────┘    └──────────────────┘         │
│  └────────────┘                             │                   │
│                                              ▼                   │
│  ┌──────────────────────────────────────────────────────┐       │
│  │              Service Layer                           │       │
│  │  ┌────────────────────────────────────────────┐     │       │
│  │  │  ProductService (Business Logic)          │     │       │
│  │  │  • Validation (price, stock rules)        │     │       │
│  │  │  • Business Rules Enforcement             │     │       │
│  │  │  • Orchestration                          │     │       │
│  │  └────────────────────────────────────────────┘     │       │
│  └──────────────────────────────────────────────────────┘       │
│                                              │                   │
│  ┌─────────────┐    ┌───────────┐          │                   │
│  │    DTOs     │    │Exceptions │          ▼                   │
│  │  AutoMapper │    │FluentValid│    ┌─────────────┐           │
│  └─────────────┘    └───────────┘    │ IUnitOfWork │           │
│                                       │ IRepository │           │
└───────────────────────────────────────┴─────────────┴───────────┘
                                              │
                                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                 MyProject.Infrastructure                         │
│  ┌────────────────┐    ┌──────────────┐   ┌────────────┐       │
│  │  UnitOfWork    │───▶│ Repository   │───▶│  DbContext │       │
│  │  (Transaction) │    │  (CRUD)      │   │ (EF Core)  │       │
│  └────────────────┘    └──────────────┘   └────────────┘       │
└─────────────────────────────────────────────────────────────────┘
                                              │
                                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                      MyProject.Domain                            │
│  ┌────────────────────────────────────────────────────┐         │
│  │  Product Entity (Domain Model)                     │         │
│  └────────────────────────────────────────────────────┘         │
└─────────────────────────────────────────────────────────────────┘
```

## Service Layer Benefits

### ✅ **Business Logic Separated from Handlers**
- Handlers remain thin and focused on orchestration
- All business rules are centralized in the Service layer
- Clear separation of concerns

### ✅ **Reusable Across Multiple Handlers or Controllers**
- Service methods can be called from anywhere
- No duplication of business logic
- Single source of truth for business rules

### ✅ **Easier Unit Testing of Business Rules**
- Test business logic independently from MediatR
- Mock IUnitOfWork for isolated testing
- Clear test boundaries

### ✅ **Clear Separation of Concerns**
- **Controllers**: HTTP concerns
- **Handlers**: Command/Query orchestration
- **Services**: Business logic
- **Repositories**: Data access

### ✅ **Handlers Remain Thin (Orchestration Only)**
- Handlers simply delegate to services
- No business logic in handlers
- Easy to understand and maintain

### ✅ **Services Contain Domain Logic**
- All validation rules
- All business rules
- Data transformation

## Business Rules Implemented

### **CreateProductAsync**
- ✅ Validates product name is unique
- ✅ Validates price >= 0
- ✅ Generates GUID for new product
- ✅ Sets CreatedAt and UpdatedAt timestamps

### **UpdateProductAsync**
- ✅ Checks product exists (throws NotFoundException)
- ✅ Validates price >= 0
- ✅ Validates name doesn't conflict with other products
- ✅ **Business Rule**: Cannot deactivate if stock < 10
- ✅ Updates timestamps

### **DeleteProductAsync**
- ✅ Checks product exists (throws NotFoundException)
- ✅ **Business Rule**: Cannot delete if stock > 0
- ✅ Returns success status

### **GetProductByIdAsync**
- ✅ Retrieves product by ID
- ✅ Throws NotFoundException if not found
- ✅ Maps to DTO

### **GetAllProductsAsync**
- ✅ Retrieves all products
- ✅ Maps to DTOs

### **GetProductsPagedAsync**
- ✅ Retrieves paginated results
- ✅ Returns PaginatedResult with metadata

### **GetActiveProductsAsync**
- ✅ Filters products where IsActive = true
- ✅ Maps to DTOs

### **IsProductNameUniqueAsync**
- ✅ Checks name uniqueness
- ✅ Supports excluding specific product ID

### **GetProductByNameAsync**
- ✅ Finds product by name
- ✅ Returns DTO or null

## Architecture Flow

```
HTTP Request
    ↓
Controller (API Layer)
    ↓
MediatR Command/Query
    ↓
Handler (Thin - Orchestration)
    ↓
Service Layer (Business Logic)
    ├─ Validation
    ├─ Business Rules
    └─ Data Orchestration
    ↓
UnitOfWork (Transaction Management)
    ↓
Repository (Data Access)
    ↓
DbContext (Entity Framework)
    ↓
Database (In-Memory)
```

## Technology Stack

- **ASP.NET Core 10** - Web API framework
- **MediatR** - CQRS implementation
- **FluentValidation** - Input validation
- **AutoMapper** - Object-to-object mapping
- **Entity Framework Core** - ORM (In-Memory database)
- **Swashbuckle** - Swagger/OpenAPI documentation

## Project Structure

```
MyProject/
├── MyProject.Domain/
│   └── Entities/
│       └── Product.cs
│
├── MyProject.Application/
│   ├── Commands/
│   │   ├── CreateProductCommand.cs
│   │   ├── UpdateProductCommand.cs
│   │   └── DeleteProductCommand.cs
│   ├── Queries/
│   │   ├── GetProductByIdQuery.cs
│   │   ├── GetAllProductsQuery.cs
│   │   └── GetProductsPagedQuery.cs
│   ├── Handlers/
│   │   ├── CreateProductCommandHandler.cs
│   │   ├── UpdateProductCommandHandler.cs
│   │   ├── DeleteProductCommandHandler.cs
│   │   ├── GetProductByIdQueryHandler.cs
│   │   ├── GetAllProductsQueryHandler.cs
│   │   └── GetProductsPagedQueryHandler.cs
│   ├── Services/           ← **SERVICE LAYER**
│   │   ├── IProductService.cs
│   │   └── ProductService.cs
│   ├── DTOs/
│   │   ├── ProductDto.cs
│   │   └── PaginatedResult.cs
│   ├── Interfaces/
│   │   ├── IUnitOfWork.cs
│   │   └── IRepository.cs
│   ├── Exceptions/
│   │   ├── NotFoundException.cs
│   │   └── ValidationException.cs
│   └── Mappings/
│       └── MappingProfile.cs
│
├── MyProject.Infrastructure/
│   ├── Persistence/
│   │   └── ApplicationDbContext.cs
│   ├── Repositories/
│   │   └── Repository.cs
│   └── UnitOfWork/
│       └── UnitOfWork.cs
│
└── MyProject.API/
    ├── Controllers/
    │   └── ProductsController.cs
    ├── Middleware/
    │   └── DuetGlobalExceptionMiddleware.cs
    └── Program.cs
```

## Getting Started

### Prerequisites
- .NET 10 SDK
- Any code editor (VS Code, Visual Studio, Rider)

### Build and Run

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run API
cd MyProject.API
dotnet run
```

### Access API Documentation
Once running, navigate to:
- **Swagger UI**: `https://localhost:5001/swagger`

## API Endpoints

### Products

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `GET /api/products/paged?pageNumber=1&pageSize=10` - Get paginated products
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

### Example Request (Create Product)

```json
{
  "name": "Gaming Laptop",
  "description": "High-performance gaming laptop",
  "price": 1299.99,
  "stock": 50,
  "isActive": true
}
```

## Exception Handling

The `DuetGlobalExceptionMiddleware` provides centralized exception handling:

- **NotFoundException** → HTTP 404
- **ValidationException** → HTTP 400
- **Other Exceptions** → HTTP 500

## Clean Architecture Principles

### ✅ **Dependency Rule**
- Domain has no dependencies
- Application depends only on Domain
- Infrastructure depends on Application and Domain
- API depends on Application and Infrastructure

### ✅ **Testability**
- Each layer can be tested independently
- Business logic is isolated in Services
- Easy to mock dependencies

### ✅ **Maintainability**
- Clear separation of concerns
- Easy to locate and modify code
- Minimal coupling between layers

### ✅ **Scalability**
- Add new features without affecting existing code
- Service layer can grow independently
- Easy to add new entities following same pattern