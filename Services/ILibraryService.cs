using LibraryManagement.DTOs;

namespace LibraryManagement.Services
{
    public interface ILibraryService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        Task<BookDto> AddBookAsync(CreateBookDto dto);

        Task<MemberDto> AddMemberAsync(CreateMemberDto dto);

        Task<LoanDto> LendBookAsync(LendBookDto dto);
        Task<LoanDto?> ReturnBookAsync(int loanId);

        Task<LibraryDto> AddLibraryAsync(CreateLibraryDto dto);
        Task<IEnumerable<LibraryDto>> GetLibrariesAsync();
        Task<LibraryDto?> GetLibraryAsync(int id);
    }
}
