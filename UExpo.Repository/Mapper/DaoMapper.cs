using AutoMapper;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Admins;
using UExpo.Domain.Entities.Agendas;
using UExpo.Domain.Entities.Calendar;
using UExpo.Domain.Entities.CallCenterChat;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Catalogs.ItemImages;
using UExpo.Domain.Entities.Catalogs.Pdfs;
using UExpo.Domain.Entities.Fairs;
using UExpo.Domain.Entities.Fairs.Segments;
using UExpo.Domain.Entities.Tutorial;
using UExpo.Domain.Entities.Users;
using UExpo.Domain.Shared.Converters;

namespace UExpo.Repository.Mapper;

public class DaoMapper : Profile
{
    public DaoMapper()
    {
        // User Module
        CreateMap<AdminDao, Admin>().ReverseMap();
        CreateMap<UserDao, User>().ReverseMap();
        CreateMap<UserImageDao, UserImage>().ReverseMap();

        // CallCenter Module
        CreateMap<CallCenterChatDao, CallCenterChat>().ReverseMap();
        CreateMap<CallCenterMessageDao, CallCenterMessage>().ReverseMap();

        // Catalog Module
        CreateMap<CatalogDao, Catalog>()
            .ForMember(dest => dest.JsonTable,
                       opt => opt.MapFrom(src => JsonConverter.JsonToDictionary(src.JsonTable)))
            .ReverseMap()
                .ForMember(dest => dest.JsonTable,
                           opt => opt.MapFrom(src => JsonConverter.DictionaryToJson(src.JsonTable)));

        CreateMap<CatalogItemImageDao, CatalogItemImage>().ReverseMap();
        CreateMap<CatalogPdfDao, CatalogPdf>().ReverseMap();

        // Fair Module
        CreateMap<AgendaDao, Agenda>().ReverseMap();
        CreateMap<FairDao, Fair>().ReverseMap();
        CreateMap<SegmentDao, Segment>().ReverseMap();

        // Calendar Module
        CreateMap<CalendarDao, Calendar>().ReverseMap();
        CreateMap<CalendarFairDao, CalendarFair>().ReverseMap();
        CreateMap<CalendarSegmentDao, CalendarSegment>().ReverseMap();

        // Tutorial Module
        CreateMap<TutorialDao, Tutorial>().ReverseMap();
    }
}
