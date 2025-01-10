using AutoMapper;
using contrarian_reads_backend.DTOs;
using contrarian_reads_backend.Models;

namespace contrarian_reads_backend.Mappings
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<ApplicationUserDto, ApplicationUser>();
        }
    }
}

