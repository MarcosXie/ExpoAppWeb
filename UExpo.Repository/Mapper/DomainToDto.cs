using AutoMapper;
using UExpo.Domain.Users;

namespace UExpo.Repository.Mapper;

public class DomainToDto : Profile
{
    public DomainToDto()
    {
        CreateMap<User, UserDto>();
    }
}
