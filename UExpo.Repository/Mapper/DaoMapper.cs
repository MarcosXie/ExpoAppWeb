using AutoMapper;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Admins;
using UExpo.Domain.Entities.Agendas;
using UExpo.Domain.Entities.Calendars;
using UExpo.Domain.Entities.Calendars.Fairs;
using UExpo.Domain.Entities.Calendars.Segments;
using UExpo.Domain.Entities.Cart;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Catalogs.ItemImages;
using UExpo.Domain.Entities.Catalogs.Pdfs;
using UExpo.Domain.Entities.Chats.CallCenterChat;
using UExpo.Domain.Entities.Chats.RelationshipChat;
using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Entities.Exhibitors;
using UExpo.Domain.Entities.Expo;
using UExpo.Domain.Entities.Fairs;
using UExpo.Domain.Entities.Fairs.Segments;
using UExpo.Domain.Entities.Relationships;
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

        // Exhibitor Module
        CreateMap<ExhibitorFairRegisterDao, ExhibitorFairRegister>().ReverseMap();

		// Visitor Module
        CreateMap<LastSearchedTagsDao, LastSearchedTags>().ReverseMap();
        CreateMap<RelationshipDao, Relationship>().ReverseMap();
        CreateMap<RelationshipMessageDao, RelationshipMessage>().ReverseMap();
        CreateMap<RelationshipMessageDao, BaseMessage>().ReverseMap();
        CreateMap<CallCenterMessageDao, BaseMessage>().ReverseMap();

		// Cart Module
		CreateMap<CartDao, Cart>().ReverseMap();
		CreateMap<CartItemDao, CartItem>().ReverseMap();
	}
}
