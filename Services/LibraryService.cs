using AutoMapper;
using LibraryManagement.Data;
using LibraryManagement.DTOs;
using LibraryManagement.Entities;
using LibraryManagement.Services;

namespace LibraryManagementSystem.Services;

public class LibraryService(
    IGenericRepository<Book> books,
    IGenericRepository<Member> members,
    IGenericRepository<Loan> loans,
    IGenericRepository<Library> libraries,
    IMapper mapper,
    ILogger<LibraryService> logger) : ILibraryService
{
    public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
    {
        var bookEntities = await books.GetAllAsync();
        return mapper.Map<IEnumerable<BookDto>>(bookEntities);
    }

    public async Task<BookDto> AddBookAsync(CreateBookDto dto)
    {
        var book = mapper.Map<Book>(dto);
        await books.AddAsync(book);
        await books.SaveAsync();

        logger.LogInformation("Book added: {Title}", book.Title);
        return mapper.Map<BookDto>(book);
    }

    public async Task<MemberDto> AddMemberAsync(CreateMemberDto dto)
    {
        var member = mapper.Map<Member>(dto);
        await members.AddAsync(member);
        await members.SaveAsync();

        logger.LogInformation("Member added: {Name}", member.Name);
        return mapper.Map<MemberDto>(member);
    }

    public async Task<LoanDto> LendBookAsync(LendBookDto dto)
    {
        var book = await books.GetByIdAsync(dto.BookId)
            ?? throw new InvalidOperationException("Book not found");

        if (!book.IsAvailable)
            throw new InvalidOperationException("Book already borrowed");

        var member = await members.GetByIdAsync(dto.MemberId)
            ?? throw new InvalidOperationException("Member not found");

        var loan = new Loan
        {
            BookId = dto.BookId,
            MemberId = dto.MemberId
        };

        book.IsAvailable = false;
        books.Update(book);
        await loans.AddAsync(loan);
        await books.SaveAsync();

        logger.LogInformation("Book {BookId} borrowed by Member {MemberId}",
            dto.BookId, dto.MemberId);

        return mapper.Map<LoanDto>(loan);
    }

    public async Task<LoanDto?> ReturnBookAsync(int loanId)
    {
        var loan = await loans.GetByIdAsync(loanId)
            ?? throw new InvalidOperationException("Loan not found");

        loan.ReturnedAt = DateTime.UtcNow;

        var book = await books.GetByIdAsync(loan.BookId);
        if (book is not null)
        {
            book.IsAvailable = true;
            books.Update(book);
        }

        loans.Update(loan);
        await loans.SaveAsync();

        logger.LogInformation("Loan {LoanId} returned", loanId);
        return mapper.Map<LoanDto>(loan);
    }

    public async Task<LibraryDto> AddLibraryAsync(CreateLibraryDto dto)
    {
        var library = mapper.Map<Library>(dto);
        await libraries.AddAsync(library);
        await libraries.SaveAsync();

        logger.LogInformation("Library added: {Name}", library.Name);
        return mapper.Map<LibraryDto>(library);
    }

    public async Task<IEnumerable<LibraryDto>> GetLibrariesAsync()
    {
        var libraryEntities = await libraries.GetAllAsync();
        return mapper.Map<IEnumerable<LibraryDto>>(libraryEntities);
    }

    public async Task<LibraryDto?> GetLibraryAsync(int id)
    {
        var library = await libraries.GetByIdAsync(id);
        return library is null ? null : mapper.Map<LibraryDto>(library);
    }
}