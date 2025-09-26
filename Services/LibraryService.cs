using AutoMapper;
using LibraryManagement.Data;
using LibraryManagement.DTOs;
using LibraryManagement.Entities;
using LibraryManagement.Services;

namespace LibraryManagementSystem.Services;

public class LibraryService : ILibraryService
{
    private readonly IGenericRepository<Book> _books;
    private readonly IGenericRepository<Member> _members;
    private readonly IGenericRepository<Loan> _loans;
    private readonly IGenericRepository<Library> _libraries;
    private readonly IMapper _mapper;

    public LibraryService(
        IGenericRepository<Book> books,
        IGenericRepository<Member> members,
        IGenericRepository<Loan> loans,
        IGenericRepository<Library> libraries,
        IMapper mapper)
    {
        _books = books;
        _members = members;
        _loans = loans;
        _libraries = libraries;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
    {
        var books = await _books.GetAllAsync();
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<BookDto> AddBookAsync(CreateBookDto dto)
    {
        if (dto.LibraryId.HasValue)
        {
            var lib = await _libraries.GetByIdAsync(dto.LibraryId.Value)
                      ?? throw new InvalidOperationException("Library not found");
        }

        var book = _mapper.Map<Book>(dto);
        await _books.AddAsync(book);
        await _books.SaveAsync();
        return _mapper.Map<BookDto>(book);
    }

    public async Task<MemberDto> AddMemberAsync(CreateMemberDto dto)
    {
        var member = _mapper.Map<Member>(dto);
        await _members.AddAsync(member);
        await _members.SaveAsync();
        return _mapper.Map<MemberDto>(member);
    }

    public async Task<LoanDto> LendBookAsync(LendBookDto dto)
    {
        var book = await _books.GetByIdAsync(dto.BookId) ?? throw new InvalidOperationException("Book not found");
        if (!book.IsAvailable) throw new InvalidOperationException("Book already borrowed");

        var loan = new Loan
        {
            BookId = dto.BookId,
            MemberId = dto.MemberId,
            BorrowedAt = DateTime.UtcNow
        };

        book.IsAvailable = false;
        _books.Update(book);
        await _loans.AddAsync(loan);

        await _books.SaveAsync(); // one save commits both changes

        return _mapper.Map<LoanDto>(loan);
    }

    public async Task<LoanDto?> ReturnBookAsync(int loanId)
    {
        var loan = await _loans.GetByIdAsync(loanId);
        if (loan == null) return null;

        loan.ReturnedAt = DateTime.UtcNow;

        var book = await _books.GetByIdAsync(loan.BookId);
        if (book != null)
        {
            book.IsAvailable = true;
            _books.Update(book);
        }
        _loans.Update(loan);

        await _loans.SaveAsync();
        return _mapper.Map<LoanDto>(loan);
    }

    public async Task<LibraryDto> AddLibraryAsync(CreateLibraryDto dto)
    {
        var library = _mapper.Map<Library>(dto);
        await _libraries.AddAsync(library);
        await _libraries.SaveAsync();
        return _mapper.Map<LibraryDto>(library);
    }

    public async Task<IEnumerable<LibraryDto>> GetLibrariesAsync()
    {
        var libs = await _libraries.GetAllAsync();
        return _mapper.Map<IEnumerable<LibraryDto>>(libs);
    }

    public async Task<LibraryDto?> GetLibraryAsync(int id)
    {
        var lib = await _libraries.GetByIdAsync(id);
        return lib == null ? null : _mapper.Map<LibraryDto>(lib);
    }
}
