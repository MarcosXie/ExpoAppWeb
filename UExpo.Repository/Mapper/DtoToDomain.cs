using AutoMapper;
using UExpo.Domain.Admins;
using UExpo.Domain.Agendas;
using UExpo.Domain.CallCenterChat;
using UExpo.Domain.Fairs;
using UExpo.Domain.Fairs.Segments;
using UExpo.Domain.Users;

namespace UExpo.Repository.Mapper;

public class DtoToDomain : Profile
{
    public DtoToDomain()
    {
        CreateMap<UserDto, User>();
        CreateMap<CallCenterChatDto, CallCenterChat>();
        CreateMap<CallCenterSendMessageDto, CallCenterMessage>();
        CreateMap<AdminDto, Admin>();

        CreateMap<AgendaDto, Agenda>();
        CreateMap<FairDto, Fair>();
        CreateMap<SegmentDto, Segment>();
    }
}
