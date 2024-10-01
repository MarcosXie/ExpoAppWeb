using AutoMapper;
using UExpo.Domain.Entities.Admins;
using UExpo.Domain.Entities.Agendas;
using UExpo.Domain.Entities.Carts;
using UExpo.Domain.Entities.Chats.CallCenterChat;
using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Entities.Expo;
using UExpo.Domain.Entities.Fairs;
using UExpo.Domain.Entities.Fairs.Segments;
using UExpo.Domain.Entities.Relationships;
using UExpo.Domain.Entities.Tutorial;
using UExpo.Domain.Entities.Users;

namespace UExpo.Repository.Mapper;

public class DtoToDomain : Profile
{
    public DtoToDomain()
    {
        CreateMap<UserDto, User>();
        CreateMap<UserProfileDto, User>()
            .ForMember(x => x.Images, opt => opt.Ignore())
            .ForMember(x => x.Password, opt => opt.Ignore());
        CreateMap<ChatDto, CallCenterChat>();
        CreateMap<SendMessageDto, CallCenterMessage>();
        CreateMap<AdminDto, Admin>();

        CreateMap<AgendaDto, Agenda>();
        CreateMap<FairDto, Fair>();
        CreateMap<SegmentDto, Segment>();
        CreateMap<TutorialDto, Tutorial>();
		CreateMap<LastSearchedTagsDto, LastSearchedTags>();
		CreateMap<RelationshipDto, Relationship>();

		CreateMap<CartDto, Cart>();
		CreateMap<CartItemDto, CartItem>();
	}
}
