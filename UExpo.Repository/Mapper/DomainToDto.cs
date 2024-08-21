using AutoMapper;
using UExpo.Domain.Admins;
using UExpo.Domain.Catalogs;
using UExpo.Domain.Catalogs.ItemImages;
using UExpo.Domain.Catalogs.Pdfs;
using UExpo.Domain.Users;

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
    }
}
