using AutoMapper;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Chats.CallCenterChat;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CallCenterMessageRepository(UExpoDbContext context, IMapper mapper)
: BaseRepository<CallCenterMessageDao, CallCenterMessage>(context, mapper), ICallCenterMessageRepository
{ 
}
