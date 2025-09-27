using LibraryManagement.Data;
using LibraryManagement.DTOs;
using LibraryManagement.Entities;
using LibraryManagement.Mapper;
using LibraryManagement.Services;
using LibraryManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;

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

// Error handling middleware
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;

        logger.LogError(error, "Unhandled exception occurred.");

        context.Response.StatusCode = ((int)HttpStatusCode.InternalServerError);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(ApiResponse.Fail("An unexpected error occurred."));
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoints

app.MapGet("/api/books", async (ILibraryService service) =>
{
    var books = await service.GetAllBooksAsync();
    return Results.Ok(ApiResponse.Ok(books));
});

app.MapPost("/api/books", async (CreateBookDto dto, ILibraryService service) =>
{
    var book = await service.AddBookAsync(dto);
    return Results.Ok(ApiResponse.Ok(book));
});

app.MapPost("/api/members", async (CreateMemberDto dto, ILibraryService service) =>
{
    var member = await service.AddMemberAsync(dto);
    return Results.Ok(ApiResponse.Ok(member));
});

app.MapPost("/api/loans/lend", async (LendBookDto dto, ILibraryService service) =>
{
    var loan = await service.LendBookAsync(dto);
    return Results.Ok(ApiResponse.Ok(loan));
});

app.MapPost("/api/loans/return/{loanId:int}", async (int loanId, ILibraryService service) =>
{
    var result = await service.ReturnBookAsync(loanId);
    return Results.Ok(ApiResponse.Ok(result));
});

app.MapPost("/api/libraries", async (CreateLibraryDto dto, ILibraryService service) =>
{
    var library = await service.AddLibraryAsync(dto);
    return Results.Ok(ApiResponse.Ok(library));
});

app.Run();
