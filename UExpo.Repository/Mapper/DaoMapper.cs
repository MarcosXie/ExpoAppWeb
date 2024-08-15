using AutoMapper;
using UExpo.Domain.Admins;
using UExpo.Domain.CallCenterChat;
using UExpo.Domain.Catalogs;
using UExpo.Domain.Catalogs.ItemImages;
using UExpo.Domain.Catalogs.Pdfs;
using UExpo.Domain.Dao;
using UExpo.Domain.Shared.Converters;
using UExpo.Domain.Users;

namespace UExpo.Repository.Mapper;

public class DaoMapper : Profile
{
    public DaoMapper()
    {
        CreateMap<UserDao, User>().ReverseMap();
        CreateMap<CallCenterChatDao, CallCenterChat>().ReverseMap();
        CreateMap<CallCenterMessageDao, CallCenterMessage>().ReverseMap();
        CreateMap<AdminDao, Admin>().ReverseMap();

        // Catalog Module
        CreateMap<CatalogDao, Catalog>()
            .ForMember(dest => dest.JsonTable,
                       opt => opt.MapFrom(src => JsonConverter.JsonToDictionary(src.JsonTable)))
            .ReverseMap()
                .ForMember(dest => dest.JsonTable,
                           opt => opt.MapFrom(src => JsonConverter.DictionaryToJson(src.JsonTable)));

        CreateMap<CatalogItemImageDao, CatalogItemImage>().ReverseMap();
        CreateMap<CatalogPdfDao, CatalogPdf>().ReverseMap();
    }
}
