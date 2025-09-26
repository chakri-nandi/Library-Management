namespace LibraryManagement.DTOs;

// Books
public record BookDto(int Id, string Title, string Author, bool IsAvailable, int? LibraryId);
public record CreateBookDto(string Title, string Author, int? LibraryId);

// Members
public record MemberDto(int Id, string Name, string Email);
public record CreateMemberDto(string Name, string Email);

// Loans
public record LoanDto(int Id, int BookId, int MemberId, DateTime BorrowedAt, DateTime? ReturnedAt);
public record LendBookDto(int BookId, int MemberId);

// Libraries
public record LibraryDto(int Id, string Name, string? Address);
public record CreateLibraryDto(string Name, string? Address);
