using AutoMapper;
using LibraryManagement.DTOs;
using LibraryManagement.Entities;

namespace LibraryManagement.Mapper
{
    public class LibraryMappingProfile : Profile
    {
        public LibraryMappingProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateBookDto, Book>();

            CreateMap<Member, MemberDto>();
            CreateMap<CreateMemberDto, Member>();

            CreateMap<Loan, LoanDto>();

            CreateMap<Library, LibraryDto>();
            CreateMap<CreateLibraryDto, Library>();
        }
    }
}
