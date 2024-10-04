using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Chats.CallCenterChat;

public interface ICallCenterMessageRepository :  IBaseRepository<CallCenterMessageDao, CallCenterMessage>
{
}
