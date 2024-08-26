using AutoMapper;
using UExpo.Domain.Entities.Admins;
using UExpo.Domain.Entities.Agendas;
using UExpo.Domain.Entities.CallCenterChat;
using UExpo.Domain.Entities.Fairs;
using UExpo.Domain.Entities.Fairs.Segments;
using UExpo.Domain.Entities.Users;

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
