using AutoMapper;
using contrarian_reads_backend.DTOs;
using contrarian_reads_backend.Models;

namespace contrarian_reads_backend.Mappings
{
    public class BookAlternativeProfile : Profile
    {
        public BookAlternativeProfile()
        {
            CreateMap<BookAlternative, BookAlternativeDto>();
        }
    }
}
