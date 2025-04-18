using AutoMapper;
using contrarian_reads_backend.Models;
using contrarian_reads_backend.Services.DTOs;

namespace contrarian_reads_backend.Profiles;

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
        CreateMap<Suggestion, SuggestionResponseDTO>();
        CreateMap<Suggestion, MinimalSuggestionDTO>()
            .ForCtorParam("SuggestedbookTitle", opt => opt.MapFrom(src => src.SuggestedBook.Title))
            .ForCtorParam("SuggestedbookAuthor", opt => opt.MapFrom(src => src.SuggestedBook.Author))
            .ForCtorParam("MainbookTitle", opt => opt.MapFrom(src => src.Book.Title))
            .ForCtorParam("MainbookAuthor", opt => opt.MapFrom(src => src.Book.Author));

        CreateMap<Suggestion, SuggestionWithCommentsDTO>()
            .ForMember(dest => dest.UpvoteCount, opt => opt.MapFrom(src => src.Upvotes.Count));

        CreateMap<CreateSuggestionDTO, Suggestion>();

        CreateMap<CreateCommentDTO, CommentDTO>();
        CreateMap<CreateCommentDTO, Comment>();

        CreateMap<Comment, CommentDTO>();
        CreateMap<CommentDTO, Comment>();
    }
}