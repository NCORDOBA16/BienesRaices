using Application.DTOs.Owners;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.Profiles
{
    public class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<CreateOwnerDto, Owner>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday));

            CreateMap<Owner, OwnerDto>()
                .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday));
        }
    }
}
