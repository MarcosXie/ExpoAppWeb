using AutoMapper;
using UExpo.Domain.Users;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Mapper;

public class DaoMapper : Profile
{
    public DaoMapper()
    {
        CreateMap<UserDao, User>().ReverseMap();
    }

}
