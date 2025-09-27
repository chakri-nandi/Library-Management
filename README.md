Library Management System
A simple console-based library management system built with .NET that allows you to manage books, members, and loan transactions.
Features

Book Management: Add, view, update, and delete books
Member Management: Register and manage library members
Loan System: Handle book borrowing and returning
Data Persistence: Uses Entity Framework Core for data storage
Clean Architecture: Implements Repository pattern

Project Structure
LibraryManagementSystem/
├── Data/                   # Data access layer
│   ├── LibraryDbContext.cs     # Database context
│   ├── IGenericRepository.cs   # Generic repository interface
│   ├── GenericRepository.cs    # Generic repository implementation
├── DTOs/                   # Data Transfer Objects
│   ├── BookDto.cs             # Book data transfer object
│   ├── MemberDto.cs           # Member data transfer object
│   ├── LoanDto.cs             # Loan data transfer object
│   ├── CreateBookDto.cs       # DTO for creating books
│   ├── CreateMemberDto.cs     # DTO for creating members
│   └── BorrowBookDto.cs       # DTO for borrowing books
├── Entities/               # Domain models
│   ├── Book.cs                # Book entity
│   ├── Member.cs              # Member entity
│   └── Loan.cs                # Loan entity
├── Mapping/                # AutoMapper profiles
│   └── MappingProfile.cs      # Entity to DTO mapping configuration
├── Services/               # Business logic layer
│   ├── ILibraryService.cs     # Library service interface
│   └── LibraryService.cs      # Library service implementation
├── Tests/                  # Unit tests
│   └── LibraryTests.cs        # Test cases
└── Program.cs              # Application entry point
