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


// 🔹 Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoints
app.MapGet("/api/books", async (ILibraryService s) => Results.Ok(await s.GetAllBooksAsync()));
app.MapPost("/api/books", async (ILibraryService s, CreateBookDto dto) => Results.Ok(await s.AddBookAsync(dto)));

app.MapPost("/api/members", async (ILibraryService s, CreateMemberDto dto) => Results.Ok(await s.AddMemberAsync(dto)));

app.MapPost("/api/loans/lend", async (ILibraryService s, LendBookDto dto) => Results.Ok(await s.LendBookAsync(dto)));
app.MapPost("/api/loans/return/{loanId:int}", async (ILibraryService s, int loanId) =>
{
    var loan = await s.ReturnBookAsync(loanId);
    return loan is null ? Results.NotFound() : Results.Ok(loan);
});

app.MapGet("/api/libraries", async (ILibraryService s) => Results.Ok(await s.GetLibrariesAsync()));
app.MapGet("/api/libraries/{id:int}", async (ILibraryService s, int id) =>
{
    var lib = await s.GetLibraryAsync(id);
    return lib is null ? Results.NotFound() : Results.Ok(lib);
});
app.MapPost("/api/libraries", async (ILibraryService s, CreateLibraryDto dto) => Results.Ok(await s.AddLibraryAsync(dto)));

app.Run();
