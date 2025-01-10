using AutoMapper;
using contrarian_reads_backend.DTOs;
using contrarian_reads_backend.Models;

namespace contrarian_reads_backend.Mappings
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>();
            //.ForMember(dest => dest.BookAlternatives, opt => opt.MapFrom(src => src.BookAlternatives))
            //.ForMember(dest => dest.AlternativeToBooks, opt => opt.MapFrom(src => src.AlternativeToBooks));

            CreateMap<BookAlternative, BookAlternativeDto>();
        }
    }
}