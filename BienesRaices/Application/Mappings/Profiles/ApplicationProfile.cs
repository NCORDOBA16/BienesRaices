using Application.DTOs.VersionControls;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.Profiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<VersionControl, VersionControlDto>()
                .ForMember(dest => dest.VersionActual, opt => opt.MapFrom(src => src.CurrentVersion))
                .ReverseMap();
        }
    }
}
