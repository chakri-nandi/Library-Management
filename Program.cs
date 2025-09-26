using LibraryManagement.Data;
using LibraryManagement.DTOs;
using LibraryManagement.Entities;
using LibraryManagement.Mapper;
using LibraryManagement.Services;
using LibraryManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryDbContext>(opt => opt.UseInMemoryDatabase("LibraryDb"));

// Register generic repositories
builder.Services.AddScoped<IGenericRepository<Book>, GenericRepository<Book>>();
builder.Services.AddScoped<IGenericRepository<Member>, GenericRepository<Member>>();
builder.Services.AddScoped<IGenericRepository<Loan>, GenericRepository<Loan>>();
builder.Services.AddScoped<IGenericRepository<Library>, GenericRepository<Library>>();

// Service layer
builder.Services.AddScoped<ILibraryService, LibraryService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(LibraryMappingProfile));

var app = builder.Build();

// Books
app.MapGet("/api/books", async (ILibraryService s) => Results.Ok(await s.GetAllBooksAsync()));
app.MapPost("/api/books", async (ILibraryService s, CreateBookDto dto) => Results.Ok(await s.AddBookAsync(dto)));

// Members
app.MapPost("/api/members", async (ILibraryService s, CreateMemberDto dto) => Results.Ok(await s.AddMemberAsync(dto)));

// Loans
app.MapPost("/api/loans/borrow", async (ILibraryService s, LendBookDto dto) => Results.Ok(await s.LendBookAsync(dto)));
app.MapPost("/api/loans/return/{loanId:int}", async (ILibraryService s, int loanId) =>
{
    var loan = await s.ReturnBookAsync(loanId);
    return loan is null ? Results.NotFound() : Results.Ok(loan);
});

// Libraries
app.MapGet("/api/libraries", async (ILibraryService s) => Results.Ok(await s.GetLibrariesAsync()));
app.MapGet("/api/libraries/{id:int}", async (ILibraryService s, int id) =>
{
    var lib = await s.GetLibraryAsync(id);
    return lib is null ? Results.NotFound() : Results.Ok(lib);
});
app.MapPost("/api/libraries", async (ILibraryService s, CreateLibraryDto dto) => Results.Ok(await s.AddLibraryAsync(dto)));

app.Run();
