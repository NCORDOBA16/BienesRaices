using Application.DTOs.Properties;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.Profiles
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<CreatePropertyDto, Property>()
             .ForMember(dest => dest.IdProperty, opt => opt.Ignore())
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
             .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Description ?? string.Empty))
             .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
             .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(_ => "system")) // cambiar por usuario real si aplica
             .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
            //.ForMember(dest => dest.PropertyImages, opt => opt.MapFrom(src =>
            //    src.ImageUrls == null
            //        ? new List<PropertyImage>()
            //        : src.ImageUrls.Select(u => new PropertyImage { File = u }).ToList()
            //));

            // Entity -> DTO (respuesta)
            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PropertyAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner != null ? new OwnerDto { IdOwner = src.Owner.IdOwner, OwnerName = src.Owner.Name, OwnerPhoto = src.Owner.Photo != null ? Convert.ToBase64String(src.Owner.Photo) : null } : null))
                .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src => src.PropertyImages != null && src.PropertyImages.Any() ? src.PropertyImages.OrderByDescending(pi => pi.CreatedAt).Select(pi => pi.File).FirstOrDefault() : null))
                .ForMember(dest => dest.TotalImages, opt => opt.MapFrom(src => src.PropertyImages != null ? src.PropertyImages.Count : 0))
                .ForMember(dest => dest.LastSaleDate, opt => opt.MapFrom(src => src.PropertyTraces != null && src.PropertyTraces.Any() ? src.PropertyTraces.OrderByDescending(t => t.DateSale).Select(t => (DateTime?)t.DateSale).FirstOrDefault() : (DateTime?)null))
                .ForMember(dest => dest.LastSaleValue, opt => opt.MapFrom(src => src.PropertyTraces != null && src.PropertyTraces.Any() ? src.PropertyTraces.OrderByDescending(t => t.DateSale).Select(t => (decimal?)t.Value).FirstOrDefault() : (decimal?)null));
                //.ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src =>
                //    src.PropertyImages != null
                //        ? src.PropertyImages.Select(pi => pi.File).ToList()
                //        : new List<string>()));
        }
    }
}
