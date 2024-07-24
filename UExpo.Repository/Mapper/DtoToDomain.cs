using AutoMapper;
using UExpo.Domain.Users;

namespace UExpo.Repository.Mapper;

public class DtoToDomain : Profile
{
    public DtoToDomain()
    {
        CreateMap<UserDto, User>();
    }
}
