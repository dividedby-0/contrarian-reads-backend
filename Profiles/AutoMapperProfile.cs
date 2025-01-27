namespace contrarian_reads_backend.Profiles;

using AutoMapper;
using contrarian_reads_backend.DTOs;
using contrarian_reads_backend.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Book, BookDTO>();
        CreateMap<BookDTO, Book>();

        CreateMap<CreateBookDTO, Book>();

        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();

        CreateMap<CreateUserDTO, User>();

        CreateMap<LoginDTO, User>();

        CreateMap<Suggestion, SuggestionDTO>();
        CreateMap<SuggestionDTO, Suggestion>();

        CreateMap<Suggestion, SuggestionWithCommentsDTO>();

        CreateMap<CreateSuggestionDTO, Suggestion>();
    }
}