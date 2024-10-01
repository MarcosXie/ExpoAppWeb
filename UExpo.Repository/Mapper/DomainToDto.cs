using AutoMapper;
using UExpo.Domain.Entities.Admins;
using UExpo.Domain.Entities.Agendas;
using UExpo.Domain.Entities.Calendars;
using UExpo.Domain.Entities.Calendars.Fairs;
using UExpo.Domain.Entities.Calendars.Segments;
using UExpo.Domain.Entities.Carts;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Catalogs.ItemImages;
using UExpo.Domain.Entities.Catalogs.Pdfs;
using UExpo.Domain.Entities.Expo;
using UExpo.Domain.Entities.Fairs;
using UExpo.Domain.Entities.Fairs.Segments;
using UExpo.Domain.Entities.Relationships;
using UExpo.Domain.Entities.Tutorial;
using UExpo.Domain.Entities.Users;

namespace UExpo.Repository.Mapper;

public class DomainToDto : Profile
{
    public DomainToDto()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserImage, UserImageResponseDto>();

        CreateMap<User, UserProfileResponseDto>()
            .ForMember(x => x.Images, opt => opt.MapFrom(src => src.Images.Select(img => img.Uri)));
        CreateMap<Catalog, CatalogResponseDto>();
        CreateMap<CatalogItemImage, CatalogItemImageResponseDto>();
        CreateMap<CatalogPdf, CatalogPdfResponseDto>();
        CreateMap<Admin, AdminResponseDto>();
        CreateMap<Agenda, AgendaResponseDto>();
        CreateMap<Fair, FairResponseDto>();
        CreateMap<Segment, SegmentResponseDto>();
        CreateMap<Calendar, CalendarResponseDto>();
        CreateMap<CalendarFair, CalendarFairResponseDto>();
        CreateMap<CalendarFair, CalendarFairOptionResponseDto>();
        CreateMap<Tutorial, TutorialResponseDto>();
        CreateMap<CalendarSegment, CalendarSegmentOptionResponseDto>();
		CreateMap<LastSearchedTags, LastSearchedTagsResponseDto>();
		CreateMap<Relationship, RelationshipResponseDto>();
		CreateMap<Cart, CartResponseDto>();
		CreateMap<CartItem, CartItemResponseDto>();
	}
}
