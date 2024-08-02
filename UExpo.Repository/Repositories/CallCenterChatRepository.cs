using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.CallCenterChat;
using UExpo.Domain.Exceptions;
using UExpo.Repository.Context;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Repositories;

public class CallCenterChatRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<CallCenterChatDao, CallCenterChat>(context, mapper), ICallCenterChatRepository
{
    public async Task<Guid> AddMessageAsync(CallCenterMessage message, CancellationToken cancellationToken = default)
    {
        var callCenter = await Database
            .FirstOrDefaultAsync(x => 
                x.Id == message.ChatId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(message.ChatId.ToString());
        var messageDao = Mapper.Map<CallCenterMessageDao>(message);

        callCenter.Messages.Add(messageDao);

        await Context.SaveChangesAsync(cancellationToken);

        return messageDao.Id;
    }
}
