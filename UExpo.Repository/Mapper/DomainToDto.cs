using AutoMapper;
using UExpo.Domain.Entities.Admins;
using UExpo.Domain.Entities.Agendas;
using UExpo.Domain.Entities.Calendar;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Catalogs.ItemImages;
using UExpo.Domain.Entities.Catalogs.Pdfs;
using UExpo.Domain.Entities.Fairs;
using UExpo.Domain.Entities.Fairs.Segments;
using UExpo.Domain.Entities.Tutorial;
using UExpo.Domain.Entities.Users;

namespace UExpo.Repository.Mapper;

public class DomainToDto : Profile
{
    public DomainToDto()
    {
        CreateMap<User, UserDto>();
        CreateMap<Catalog, CatalogResponseDto>();
        CreateMap<CatalogItemImage, CatalogItemImageResponseDto>();
        CreateMap<CatalogPdf, CatalogPdfResponseDto>();
        CreateMap<Admin, AdminResponseDto>();
        CreateMap<Agenda, AgendaResponseDto>();
        CreateMap<Fair, FairResponseDto>();
        CreateMap<Segment, SegmentResponseDto>();
        CreateMap<Calendar, CalendarReponseDto>();
        CreateMap<CalendarFair, CalendarFairResponseDto>();
        CreateMap<Tutorial, TutorialResponseDto>();
    }
}
