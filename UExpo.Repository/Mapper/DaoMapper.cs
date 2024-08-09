using AutoMapper;
using UExpo.Domain.Admins;
using UExpo.Domain.CallCenterChat;
using UExpo.Domain.Users;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Mapper;

public class DaoMapper : Profile
{
    public DaoMapper()
    {
        CreateMap<UserDao, User>().ReverseMap();
        CreateMap<CallCenterChatDao, CallCenterChat>().ReverseMap();
        CreateMap<CallCenterMessageDao, CallCenterMessage>().ReverseMap();
        CreateMap<AdminDao, Admin>().ReverseMap();
    }
}
