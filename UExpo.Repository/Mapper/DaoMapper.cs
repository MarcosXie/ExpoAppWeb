using AutoMapper;
using UExpo.Domain.User;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Mapper;

public class DaoMapper : Profile
{
    public DaoMapper()
    {
        CreateMap<UserDao, User>().ReverseMap();
    }

}
